using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeltmanSoftwareDesign.Data.Migrations
{
    /// <inheritdoc />
    public partial class verkeerdeforeignkeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workorders_Customers_CompanyId",
                table: "Workorders");

            migrationBuilder.DropForeignKey(
                name: "FK_Workorders_Projects_CompanyId",
                table: "Workorders");

            migrationBuilder.CreateIndex(
                name: "IX_Workorders_CustomerId",
                table: "Workorders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Workorders_ProjectId",
                table: "Workorders",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workorders_Customers_CustomerId",
                table: "Workorders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workorders_Projects_ProjectId",
                table: "Workorders",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workorders_Customers_CustomerId",
                table: "Workorders");

            migrationBuilder.DropForeignKey(
                name: "FK_Workorders_Projects_ProjectId",
                table: "Workorders");

            migrationBuilder.DropIndex(
                name: "IX_Workorders_CustomerId",
                table: "Workorders");

            migrationBuilder.DropIndex(
                name: "IX_Workorders_ProjectId",
                table: "Workorders");

            migrationBuilder.AddForeignKey(
                name: "FK_Workorders_Customers_CompanyId",
                table: "Workorders",
                column: "CompanyId",
                principalTable: "Customers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workorders_Projects_CompanyId",
                table: "Workorders",
                column: "CompanyId",
                principalTable: "Projects",
                principalColumn: "id");
        }
    }
}
