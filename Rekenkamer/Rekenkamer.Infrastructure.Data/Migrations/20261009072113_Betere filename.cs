using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rekenkamer.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Beterefilename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Documents",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Documents",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Documents",
                newName: "FileName");
        }
    }
}
