using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rekenkamer.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Longtoguid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Voeg tijdelijke UUID kolommen toe
            migrationBuilder.AddColumn<Guid>(name: "NewId", table: "Documents", type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()");
            migrationBuilder.AddColumn<Guid>(name: "NewId", table: "DocumentChunks", type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()");
            migrationBuilder.AddColumn<Guid>(name: "NewDocumentId", table: "DocumentChunks", type: "uuid", nullable: true); // Tijdelijk nullable voor de migratie

            // 2. Data overzetten en relatie herstellen via SQL
            // We koppelen DocumentChunks aan Documents op basis van de oude 'DocumentId' en kopiëren de 'NewId' van Documents naar 'NewDocumentId'
            migrationBuilder.Sql(@"
                UPDATE ""DocumentChunks"" dc
                SET ""NewDocumentId"" = d.""NewId""
                FROM ""Documents"" d
                WHERE dc.""DocumentId"" = d.""Id"";
            ");

            // Maak de FK kolom nu verplicht (nullable: false) aangezien alle data is overgezet
            migrationBuilder.AlterColumn<Guid>(name: "NewDocumentId", table: "DocumentChunks", type: "uuid", nullable: false);

            // 3. Verwijder de oude Constraints en Kolommen
            // Let op: EF Core maakt vaak automatisch namen aan zoals 'PK_Documents'. Controleer of deze kloppen met jouw database!
            migrationBuilder.DropForeignKey(name: "FK_DocumentChunks_Documents_DocumentId", table: "DocumentChunks");
            migrationBuilder.DropPrimaryKey(name: "PK_DocumentChunks", table: "DocumentChunks");
            migrationBuilder.DropPrimaryKey(name: "PK_Documents", table: "Documents");

            migrationBuilder.DropColumn(name: "Id", table: "Documents");
            migrationBuilder.DropColumn(name: "Id", table: "DocumentChunks");
            migrationBuilder.DropColumn(name: "DocumentId", table: "DocumentChunks");

            // 4. Hernoem de nieuwe kolommen naar de originele namen
            migrationBuilder.RenameColumn(name: "NewId", table: "Documents", newName: "Id");
            migrationBuilder.RenameColumn(name: "NewId", table: "DocumentChunks", newName: "Id");
            migrationBuilder.RenameColumn(name: "NewDocumentId", table: "DocumentChunks", newName: "DocumentId");

            // 5. Herstel de Primary Keys en Foreign Key constraints
            migrationBuilder.AddPrimaryKey(name: "PK_Documents", table: "Documents", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_DocumentChunks", table: "DocumentChunks", column: "Id");
            migrationBuilder.AddForeignKey(
                name: "FK_DocumentChunks_Documents_DocumentId",
                table: "DocumentChunks",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Aangezien data-conversie van UUID terug naar Bigint (Identity) destructief is
            // (je verliest de originele opeenvolgende IDs), gooien we hier een uitzondering.
            throw new NotSupportedException("Afwaarderen van UUID naar Bigint wordt niet ondersteund zonder dataverlies.");
        }
    }
}
