using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LibraryApp.Migrations
{
    /// <inheritdoc />
    public partial class Scheduling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Scheduling",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Day = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Hours = table.Column<List<string>>(type: "text[]", nullable: false),
                    IsScheduled = table.Column<List<bool>>(type: "boolean[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scheduling", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Scheduling");
        }
    }
}
