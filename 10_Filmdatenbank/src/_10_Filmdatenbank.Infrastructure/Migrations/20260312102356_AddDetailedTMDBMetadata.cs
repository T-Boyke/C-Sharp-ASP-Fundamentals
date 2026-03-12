using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _10_Filmdatenbank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDetailedTMDBMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Biografie",
                table: "Personen",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Geburtsdatum",
                table: "Personen",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilBildUrl",
                table: "Personen",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Erscheinungsdatum",
                table: "Filme",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FskRating",
                table: "Filme",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Filme",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Handlung",
                table: "Filme",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Nutzerwertung",
                table: "Filme",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PosterUrl",
                table: "Filme",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Biografie",
                table: "Personen");

            migrationBuilder.DropColumn(
                name: "Geburtsdatum",
                table: "Personen");

            migrationBuilder.DropColumn(
                name: "ProfilBildUrl",
                table: "Personen");

            migrationBuilder.DropColumn(
                name: "Erscheinungsdatum",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "FskRating",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "Handlung",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "Nutzerwertung",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "PosterUrl",
                table: "Filme");
        }
    }
}
