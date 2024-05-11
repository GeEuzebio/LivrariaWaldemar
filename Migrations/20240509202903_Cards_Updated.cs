using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Migrations
{
    /// <inheritdoc />
    public partial class Cards_Updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "RegistrationNumber",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Card");

            migrationBuilder.AddColumn<byte[]>(
                name: "CardImage",
                table: "Card",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardImage",
                table: "Card");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Card",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Card",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistrationNumber",
                table: "Card",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Card",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
