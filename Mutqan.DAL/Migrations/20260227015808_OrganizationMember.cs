using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mutqan.DAL.Migrations
{
    /// <inheritdoc />
    public partial class OrganizationMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Organizations_OrganizationId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOrganizationHistories_User_UserId",
                table: "UserOrganizationHistories");

            migrationBuilder.DropIndex(
                name: "IX_UserOrganizationHistories_UserId",
                table: "UserOrganizationHistories");

            migrationBuilder.DropIndex(
                name: "IX_User_OrganizationId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserOrganizationHistories");

            migrationBuilder.DropColumn(
                name: "JoinedOrganizationAt",
                table: "User");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "User");

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationMemberId",
                table: "UserOrganizationHistories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProjectMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "OrganizationMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationMembers_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganizationMembers_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserOrganizationHistories_OrganizationMemberId",
                table: "UserOrganizationHistories",
                column: "OrganizationMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationMembers_OrganizationId",
                table: "OrganizationMembers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationMembers_UserId",
                table: "OrganizationMembers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrganizationHistories_OrganizationMembers_OrganizationMemberId",
                table: "UserOrganizationHistories",
                column: "OrganizationMemberId",
                principalTable: "OrganizationMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserOrganizationHistories_OrganizationMembers_OrganizationMemberId",
                table: "UserOrganizationHistories");

            migrationBuilder.DropTable(
                name: "OrganizationMembers");

            migrationBuilder.DropIndex(
                name: "IX_UserOrganizationHistories_OrganizationMemberId",
                table: "UserOrganizationHistories");

            migrationBuilder.DropColumn(
                name: "OrganizationMemberId",
                table: "UserOrganizationHistories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProjectMembers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserOrganizationHistories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinedOrganizationAt",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "User",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserOrganizationHistories_UserId",
                table: "UserOrganizationHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_OrganizationId",
                table: "User",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Organizations_OrganizationId",
                table: "User",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrganizationHistories_User_UserId",
                table: "UserOrganizationHistories",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
