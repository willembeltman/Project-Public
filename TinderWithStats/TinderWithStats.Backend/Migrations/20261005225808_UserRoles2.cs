using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinderWithStats.Backend.Migrations
{
    /// <inheritdoc />
    public partial class UserRoles2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessage_Match_MatchId",
                table: "ChatMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessage_Profiles_ProfileReceiverId",
                table: "ChatMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessage_Profiles_ProfileSenderId",
                table: "ChatMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Profiles_ProfileReceiverId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Profiles_ProfileSenderId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Role_RoleId",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Users_UserId",
                table: "UserRole");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_UserId",
                table: "Profiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Match",
                table: "Match");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatMessage",
                table: "ChatMessage");

            migrationBuilder.RenameTable(
                name: "UserRole",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "Match",
                newName: "Matches");

            migrationBuilder.RenameTable(
                name: "ChatMessage",
                newName: "ChatMessages");

            migrationBuilder.RenameIndex(
                name: "IX_UserRole_UserId",
                table: "UserRoles",
                newName: "IX_UserRoles_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRoles",
                newName: "IX_UserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_ProfileSenderId",
                table: "Matches",
                newName: "IX_Matches_ProfileSenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_ProfileReceiverId",
                table: "Matches",
                newName: "IX_Matches_ProfileReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessage_ProfileSenderId",
                table: "ChatMessages",
                newName: "IX_ChatMessages_ProfileSenderId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessage_ProfileReceiverId",
                table: "ChatMessages",
                newName: "IX_ChatMessages_ProfileReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessage_MatchId",
                table: "ChatMessages",
                newName: "IX_ChatMessages_MatchId");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "MatchRemoved",
                table: "Matches",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matches",
                table: "Matches",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatMessages",
                table: "ChatMessages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId",
                table: "Profiles",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Matches_MatchId",
                table: "ChatMessages",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Profiles_ProfileReceiverId",
                table: "ChatMessages",
                column: "ProfileReceiverId",
                principalTable: "Profiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Profiles_ProfileSenderId",
                table: "ChatMessages",
                column: "ProfileSenderId",
                principalTable: "Profiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Profiles_ProfileReceiverId",
                table: "Matches",
                column: "ProfileReceiverId",
                principalTable: "Profiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Profiles_ProfileSenderId",
                table: "Matches",
                column: "ProfileSenderId",
                principalTable: "Profiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Matches_MatchId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Profiles_ProfileReceiverId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Profiles_ProfileSenderId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Profiles_ProfileReceiverId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Profiles_ProfileSenderId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_UserId",
                table: "Profiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matches",
                table: "Matches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatMessages",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "MatchRemoved",
                table: "Matches");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserRole");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Role");

            migrationBuilder.RenameTable(
                name: "Matches",
                newName: "Match");

            migrationBuilder.RenameTable(
                name: "ChatMessages",
                newName: "ChatMessage");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRole",
                newName: "IX_UserRole_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRole",
                newName: "IX_UserRole_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_ProfileSenderId",
                table: "Match",
                newName: "IX_Match_ProfileSenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_ProfileReceiverId",
                table: "Match",
                newName: "IX_Match_ProfileReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessages_ProfileSenderId",
                table: "ChatMessage",
                newName: "IX_ChatMessage_ProfileSenderId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessages_ProfileReceiverId",
                table: "ChatMessage",
                newName: "IX_ChatMessage_ProfileReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessages_MatchId",
                table: "ChatMessage",
                newName: "IX_ChatMessage_MatchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Match",
                table: "Match",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatMessage",
                table: "ChatMessage",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId",
                table: "Profiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessage_Match_MatchId",
                table: "ChatMessage",
                column: "MatchId",
                principalTable: "Match",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessage_Profiles_ProfileReceiverId",
                table: "ChatMessage",
                column: "ProfileReceiverId",
                principalTable: "Profiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessage_Profiles_ProfileSenderId",
                table: "ChatMessage",
                column: "ProfileSenderId",
                principalTable: "Profiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Profiles_ProfileReceiverId",
                table: "Match",
                column: "ProfileReceiverId",
                principalTable: "Profiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Profiles_ProfileSenderId",
                table: "Match",
                column: "ProfileSenderId",
                principalTable: "Profiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Role_RoleId",
                table: "UserRole",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Users_UserId",
                table: "UserRole",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
