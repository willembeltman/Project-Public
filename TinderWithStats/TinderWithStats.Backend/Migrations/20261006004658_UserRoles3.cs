using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinderWithStats.Backend.Migrations
{
    /// <inheritdoc />
    public partial class UserRoles3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserRoles");
            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "UserRoles",
                type: "bigint",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "UserRoles",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
