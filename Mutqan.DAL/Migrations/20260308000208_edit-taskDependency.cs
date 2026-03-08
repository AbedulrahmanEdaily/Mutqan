using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mutqan.DAL.Migrations
{
    /// <inheritdoc />
    public partial class edittaskDependency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "TaskDependencies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TaskDependencies",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "TaskDependencies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TaskDependencies");
        }
    }
}
