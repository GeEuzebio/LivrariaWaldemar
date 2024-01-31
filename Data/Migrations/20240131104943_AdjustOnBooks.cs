using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdjustOnBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "Edition",
                table: "Book",
                newName: "Genre");

            migrationBuilder.AddColumn<long>(
                name: "Register",
                table: "Book",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Register",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "Book",
                newName: "Edition");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Book",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Book",
                type: "int",
                nullable: true);
        }
    }
}
