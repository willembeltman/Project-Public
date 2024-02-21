using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeltmanSoftwareDesign.Data.Migrations
{
    /// <inheritdoc />
    public partial class eventesten : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceWorkorders_Rates_RateId",
                table: "InvoiceWorkorders");

            migrationBuilder.RenameColumn(
                name: "RateId",
                table: "InvoiceWorkorders",
                newName: "Rateid");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceWorkorders_RateId",
                table: "InvoiceWorkorders",
                newName: "IX_InvoiceWorkorders_Rateid");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceWorkorders_Rates_Rateid",
                table: "InvoiceWorkorders",
                column: "Rateid",
                principalTable: "Rates",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceWorkorders_Rates_Rateid",
                table: "InvoiceWorkorders");

            migrationBuilder.RenameColumn(
                name: "Rateid",
                table: "InvoiceWorkorders",
                newName: "RateId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceWorkorders_Rateid",
                table: "InvoiceWorkorders",
                newName: "IX_InvoiceWorkorders_RateId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceWorkorders_Rates_RateId",
                table: "InvoiceWorkorders",
                column: "RateId",
                principalTable: "Rates",
                principalColumn: "id");
        }
    }
}
