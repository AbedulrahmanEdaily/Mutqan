using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mutqan.DAL.Migrations
{
    /// <inheritdoc />
    public partial class editprojectTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "Tasks",
                newName: "EstimatedStartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualEndDate",
                table: "Tasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualStartDate",
                table: "Tasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EstimatedEndDate",
                table: "Tasks",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualEndDate",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ActualStartDate",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "EstimatedEndDate",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "EstimatedStartDate",
                table: "Tasks",
                newName: "DueDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
