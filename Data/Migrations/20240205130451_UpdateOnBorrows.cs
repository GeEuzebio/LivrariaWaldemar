using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOnBorrows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrivateKey",
                table: "Borrow",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicKey",
                table: "Borrow",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrivateKey",
                table: "Borrow");

            migrationBuilder.DropColumn(
                name: "PublicKey",
                table: "Borrow");
        }
    }
}
