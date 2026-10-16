using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rekenkamer.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Anderedingeruit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmbeddingF32",
                table: "DocumentChunks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "EmbeddingF32",
                table: "DocumentChunks",
                type: "bytea",
                nullable: true);
        }
    }
}
