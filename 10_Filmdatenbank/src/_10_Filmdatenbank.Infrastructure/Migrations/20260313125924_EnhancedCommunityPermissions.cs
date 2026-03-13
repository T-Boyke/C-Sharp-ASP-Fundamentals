using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _10_Filmdatenbank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EnhancedCommunityPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanEditGroupContent",
                table: "GroupMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "FanGroups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresApproval",
                table: "FanGroups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "GroupBans",
                columns: table => new
                {
                    GroupBanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FanGroupID = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BannedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupBans", x => x.GroupBanID);
                    table.ForeignKey(
                        name: "FK_GroupBans_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GroupBans_FanGroups_FanGroupID",
                        column: x => x.FanGroupID,
                        principalTable: "FanGroups",
                        principalColumn: "FanGroupID");
                });

            migrationBuilder.CreateTable(
                name: "MembershipRequests",
                columns: table => new
                {
                    MembershipRequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FanGroupID = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipRequests", x => x.MembershipRequestID);
                    table.ForeignKey(
                        name: "FK_MembershipRequests_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MembershipRequests_FanGroups_FanGroupID",
                        column: x => x.FanGroupID,
                        principalTable: "FanGroups",
                        principalColumn: "FanGroupID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupBans_FanGroupID",
                table: "GroupBans",
                column: "FanGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupBans_UserID",
                table: "GroupBans",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipRequests_FanGroupID",
                table: "MembershipRequests",
                column: "FanGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipRequests_UserID",
                table: "MembershipRequests",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupBans");

            migrationBuilder.DropTable(
                name: "MembershipRequests");

            migrationBuilder.DropColumn(
                name: "CanEditGroupContent",
                table: "GroupMembers");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "FanGroups");

            migrationBuilder.DropColumn(
                name: "RequiresApproval",
                table: "FanGroups");
        }
    }
}
