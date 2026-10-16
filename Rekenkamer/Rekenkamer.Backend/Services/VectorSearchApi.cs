using gAPI.Core.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using Rekenkamer.Shared.Dtos;
using Rekenkamer.Shared.Interfaces;

namespace Rekenkamer.Backend.Services;

public class VectorSearchApi(
    IDbContextFactory<Infrastructure.Data.Entities.ApplicationDbContext> dbContextFactory,
    gAPI.Core.Interfaces.IUseCase<Infrastructure.Data.Entities.Document, Document, Guid> documentsUseCase,
    gAPI.Core.Interfaces.IUseCase<Infrastructure.Data.Entities.DocumentChunk, DocumentChunk, Guid> documentChunksUseCase,
    gAPI.Core.Interfaces.Mapping<Infrastructure.Data.Entities.Document, Document> documentsMapping,
    gAPI.Core.Interfaces.Mapping<Infrastructure.Data.Entities.DocumentChunk, DocumentChunk> documentChunksMapping,
    IEmbeddingGenerator<string, Embedding<float>> generator)
    : IVectorSearchApi
{
    public async Task<BaseListResponseT<VectorSearchResult>> VectorSearchAsync(string query, int skip, int take, CancellationToken ct)
    {
        if (await documentsUseCase.CanListAsync(ct) == false ||
            await documentChunksUseCase.CanListAsync(ct) == false ||
            string.IsNullOrWhiteSpace(query))
            return new BaseListResponseT<VectorSearchResult>()
            {
                Error = gAPI.Core.Enums.BaseResponseErrorEnum.ErrorNotAuthorized
            };

        string searchQuery = $"search_query: {query}";
        ReadOnlyMemory<float> vector = await generator.GenerateVectorAsync(searchQuery);
        var queryEmbedding = new Vector(vector.ToArray());

        using var db = dbContextFactory.CreateDbContext();
        var dbDocumentGroups = await db.Documents
            .Select(d => new
            {
                Document = d,
                // Sorteer de chunks van dit document op afstand en pak de allersnelste match
                BestChunkWithDistance = d.Chunks
                    .Select(c => new { Chunk = c, Distance = (double)c.Vector!.CosineDistance(queryEmbedding) })
                    .OrderBy(c => c.Distance)
                    .FirstOrDefault(),
                // Haal direct alle chunks op zodat de API-call in de loop weg kan
                AllChunks = d.Chunks.ToArray()
            })
            // Filter documenten die geen chunks of vectoren hebben
            .Where(x => x.BestChunkWithDistance != null)
            // Sorteer het hoofdraster op de afstand van de beste chunk
            .OrderBy(x => x.BestChunkWithDistance!.Distance)
            .Skip(skip)
            .Take(take)
            .ToArrayAsync(ct);

        var responseList = new List<VectorSearchResult>();

        foreach (var item in dbDocumentGroups)
        {
            ct.ThrowIfCancellationRequested();

            var matchingDocumentChunkDto = await documentChunksMapping.ToDtoAsync(item.BestChunkWithDistance!.Chunk, new(), ct);
            if (await documentChunksUseCase.CanReadAsync(matchingDocumentChunkDto, ct) == false)
                continue;

            var documentDto = await documentsMapping.ToDtoAsync(item.Document, new(), ct);
            if (await documentsUseCase.CanReadAsync(documentDto, ct) == false)
                continue;

            var documentChunkDtos = new List<DocumentChunk>();
            var breaked = false;
            foreach (var chunk in item.AllChunks)
            {
                var chunkDto = await documentChunksMapping.ToDtoAsync(chunk, new(), ct);
                if (await documentChunksUseCase.CanReadAsync(chunkDto, ct) == false)
                {
                    breaked = true;
                    break;
                }
                documentChunkDtos.Add(chunkDto);
            }
            if (breaked)
                continue;

            var documentGroup = new VectorSearchResult()
            {
                Document = documentDto,
                MatchingDocumentChunk = matchingDocumentChunkDto,
                DocumentChunks = documentChunkDtos.ToArray(),
                Distance = item.BestChunkWithDistance.Distance
            };

            responseList.Add(documentGroup);
        }

        return new BaseListResponseT<VectorSearchResult>()
        {
            Success = true,
            Response = responseList.ToArray()
        };
    }
    public async Task<BaseListResponseT<VectorSearchResult>> HybridSearchAsync(string query, int skip, int take, CancellationToken ct)
    {
        try
        {
            if (await documentsUseCase.CanListAsync(ct) == false ||
                await documentChunksUseCase.CanListAsync(ct) == false ||
                string.IsNullOrWhiteSpace(query))
                return new BaseListResponseT<VectorSearchResult>()
                {
                    Error = gAPI.Core.Enums.BaseResponseErrorEnum.ErrorNotAuthorized
                };

            // 1. Genereer de embedding voor de vector-zoekopdracht
            string searchQuery = $"search_query: {query}";
            ReadOnlyMemory<float> vector = await generator.GenerateVectorAsync(searchQuery);
            var queryEmbedding = new Pgvector.Vector(vector.ToArray()); // Zorg dat de type-cast klopt met jouw namespace

            // 2. Formatteer de tekstzoekopdracht voor Postgres Full-Text Search (woorden splitsen met een OR '|' operator)
            string textSearchQuery = string.Join(" | ", query.Split(' ', StringSplitOptions.RemoveEmptyEntries));

            using var db = dbContextFactory.CreateDbContext();

            // 3. Voer het Reciprocal Rank Fusion (RRF) wiskundige trucje uit via Raw SQL op de database.
            // Dit haalt de top DocumentId's op gesorteerd op een perfecte 50/50 balans tussen tekst en vector.
            var rrfSql = @"
        WITH vector_search AS (
            SELECT ""DocumentId"", ROW_NUMBER() OVER (ORDER BY ""Vector"" <=> @embedding::vector) AS rank
            FROM ""DocumentChunks""
            LIMIT 100
        ),
        text_search AS (
            SELECT ""DocumentId"", ROW_NUMBER() OVER (ORDER BY ts_rank(to_tsvector('dutch', ""Content""), to_tsquery('dutch', @textQuery)) DESC) AS rank
            FROM ""DocumentChunks""
            WHERE to_tsvector('dutch', ""Content"") @@ to_tsquery('dutch', @textQuery)
            LIMIT 100
        )
        SELECT ""DocumentId"", MAX(rrf_score) as max_score FROM (
            SELECT ""DocumentId"", 1.0 / (60.0 + rank) AS rrf_score FROM vector_search
            UNION ALL
            SELECT ""DocumentId"", 1.0 / (60.0 + rank) AS rrf_score FROM text_search
        ) sub
        GROUP BY ""DocumentId""
        ORDER BY max_score DESC";

            var parameters = new[]
            {
            new Npgsql.NpgsqlParameter("@embedding", queryEmbedding.ToArray()),
            new Npgsql.NpgsqlParameter("@textQuery", textSearchQuery)
        };

            // Voer de RRF ranking uit en haal de gesorteerde ID's op
            var rankedDocumentIds = await db.Database
                .SqlQueryRaw<Guid>(rrfSql, parameters)
                .ToListAsync(ct);

            if (!rankedDocumentIds.Any())
            {
                return new BaseListResponseT<VectorSearchResult>() { Success = true, Response = Array.Empty<VectorSearchResult>() };
            }

            // 4. Trek de volledige document-objecten en chunks uit de database op basis van de gesorteerde ID's
            var dbDocuments = await db.Documents
                .Where(d => rankedDocumentIds.Contains(d.Id))
                .Select(d => new
                {
                    Document = d,
                    BestChunkWithDistance = d.Chunks
                        .Select(c => new { Chunk = c, Distance = (double)c.Vector!.CosineDistance(queryEmbedding) })
                        .OrderBy(c => c.Distance)
                        .FirstOrDefault(),
                    AllChunks = d.Chunks.ToArray()
                })
                .Where(x => x.BestChunkWithDistance != null)
                .ToArrayAsync(ct);

            // Sorteer de database-resultaten exact in de volgorde van de RRF-ranking van Postgres, en pas skip/take toe
            var dbDocumentGroups = dbDocuments
                .OrderBy(x => rankedDocumentIds.IndexOf(x.Document.Id))
                .Skip(skip)
                .Take(take)
                .ToArray();

            // 5. De rest van je originele code blijft 100% identiek voor alle gAPI validaties en DTO mappings!
            var responseList = new List<VectorSearchResult>();

            foreach (var item in dbDocumentGroups)
            {
                ct.ThrowIfCancellationRequested();

                var matchingDocumentChunkDto = await documentChunksMapping.ToDtoAsync(item.BestChunkWithDistance!.Chunk, new(), ct);
                if (await documentChunksUseCase.CanReadAsync(matchingDocumentChunkDto, ct) == false)
                    continue;

                var documentDto = await documentsMapping.ToDtoAsync(item.Document, new(), ct);
                if (await documentsUseCase.CanReadAsync(documentDto, ct) == false)
                    continue;

                var documentChunkDtos = new List<DocumentChunk>();
                var breaked = false;
                foreach (var chunk in item.AllChunks)
                {
                    var chunkDto = await documentChunksMapping.ToDtoAsync(chunk, new(), ct);
                    if (await documentChunksUseCase.CanReadAsync(chunkDto, ct) == false)
                    {
                        breaked = true;
                        break;
                    }
                    documentChunkDtos.Add(chunkDto);
                }
                if (breaked)
                    continue;

                var documentGroup = new VectorSearchResult()
                {
                    Document = documentDto,
                    MatchingDocumentChunk = matchingDocumentChunkDto,
                    DocumentChunks = documentChunkDtos.ToArray(),
                    Distance = item.BestChunkWithDistance.Distance // Behoudt de cosine distance voor weergave in de UI
                };

                responseList.Add(documentGroup);
            }

            return new BaseListResponseT<VectorSearchResult>()
            {
                Success = true,
                Response = responseList.ToArray()
            };
        }
        catch (Exception ex)
        {
            return new BaseListResponseT<VectorSearchResult>()
            {
                Error = gAPI.Core.Enums.BaseResponseErrorEnum.ErrorNotSpecified
            };
        }
    }

}

