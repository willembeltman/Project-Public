using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UwvLlm.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ips",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    RegisterLockedOutDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RegisterCount = table.Column<int>(type: "int", nullable: false),
                    LoginLockedOutDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LoginAttempts = table.Column<int>(type: "int", nullable: false),
                    ForgetPasswordLockedOutDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ForgetPasswordAttempts = table.Column<int>(type: "int", nullable: false),
                    ChangePasswordAttempts = table.Column<int>(type: "int", nullable: false),
                    ChangePasswordLockedOutDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ips", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    LockedOut = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MailMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AutoResponse = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MailMessages_Users_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MailMessages_Users_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserIps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IpId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserIps_Ips_IpId",
                        column: x => x.IpId,
                        principalTable: "Ips",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserIps_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserNotifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalType = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuickOptions = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserNotifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenHash = table.Column<string>(type: "nvarchar(280)", maxLength: 280, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserIpSessions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserIpId = table.Column<long>(type: "bigint", nullable: false),
                    SessionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIpSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserIpSessions_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserIpSessions_UserIps_UserIpId",
                        column: x => x.UserIpId,
                        principalTable: "UserIps",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserIpSessionTokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TokenId = table.Column<long>(type: "bigint", nullable: true),
                    UserIpSessionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIpSessionTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserIpSessionTokens_UserIpSessions_UserIpSessionId",
                        column: x => x.UserIpSessionId,
                        principalTable: "UserIpSessions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserIpSessionTokens_UserTokens_TokenId",
                        column: x => x.TokenId,
                        principalTable: "UserTokens",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserIpSessionTokenRoutes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserIpSessionTokenId = table.Column<long>(type: "bigint", nullable: false),
                    RouteId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIpSessionTokenRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserIpSessionTokenRoutes_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserIpSessionTokenRoutes_UserIpSessionTokens_UserIpSessionTokenId",
                        column: x => x.UserIpSessionTokenId,
                        principalTable: "UserIpSessionTokens",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserIpSessionTokenRouteRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserIpSessionTokenRouteId = table.Column<long>(type: "bigint", nullable: false),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    Month = table.Column<byte>(type: "tinyint", nullable: false),
                    Day = table.Column<byte>(type: "tinyint", nullable: false),
                    Hour = table.Column<byte>(type: "tinyint", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIpSessionTokenRouteRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserIpSessionTokenRouteRequests_UserIpSessionTokenRoutes_UserIpSessionTokenRouteId",
                        column: x => x.UserIpSessionTokenRouteId,
                        principalTable: "UserIpSessionTokenRoutes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MailMessages_FromUserId",
                table: "MailMessages",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MailMessages_ToUserId",
                table: "MailMessages",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserIps_IpId",
                table: "UserIps",
                column: "IpId");

            migrationBuilder.CreateIndex(
                name: "IX_UserIps_UserId",
                table: "UserIps",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserIpSessions_SessionId",
                table: "UserIpSessions",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserIpSessions_UserIpId",
                table: "UserIpSessions",
                column: "UserIpId");

            migrationBuilder.CreateIndex(
                name: "IX_UserIpSessionTokenRouteRequests_UserIpSessionTokenRouteId",
                table: "UserIpSessionTokenRouteRequests",
                column: "UserIpSessionTokenRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_UserIpSessionTokenRoutes_RouteId",
                table: "UserIpSessionTokenRoutes",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_UserIpSessionTokenRoutes_UserIpSessionTokenId",
                table: "UserIpSessionTokenRoutes",
                column: "UserIpSessionTokenId");

            migrationBuilder.CreateIndex(
                name: "IX_UserIpSessionTokens_TokenId",
                table: "UserIpSessionTokens",
                column: "TokenId");

            migrationBuilder.CreateIndex(
                name: "IX_UserIpSessionTokens_UserIpSessionId",
                table: "UserIpSessionTokens",
                column: "UserIpSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_UserId",
                table: "UserNotifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_UserId",
                table: "UserTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailMessages");

            migrationBuilder.DropTable(
                name: "UserIpSessionTokenRouteRequests");

            migrationBuilder.DropTable(
                name: "UserNotifications");

            migrationBuilder.DropTable(
                name: "UserIpSessionTokenRoutes");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "UserIpSessionTokens");

            migrationBuilder.DropTable(
                name: "UserIpSessions");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "UserIps");

            migrationBuilder.DropTable(
                name: "Ips");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
