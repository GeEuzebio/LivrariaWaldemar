using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Migrations
{
    /// <inheritdoc />
    public partial class SchedulingAdjust : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Day",
                table: "Scheduling",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<int?[]>(
                name: "ClassRoom",
                table: "Scheduling",
                type: "integer[]",
                nullable: false,
                defaultValue: new int?[0]);

            migrationBuilder.AddColumn<int?[]>(
                name: "Professor",
                table: "Scheduling",
                type: "integer[]",
                nullable: false,
                defaultValue: new int?[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassRoom",
                table: "Scheduling");

            migrationBuilder.DropColumn(
                name: "Professor",
                table: "Scheduling");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Day",
                table: "Scheduling",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
