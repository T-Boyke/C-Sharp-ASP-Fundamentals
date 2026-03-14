using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _10_Filmdatenbank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FanaticMetadataStandardization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Awards",
                table: "Filme");

            migrationBuilder.RenameColumn(
                name: "Iso_639_1",
                table: "Languages",
                newName: "Iso639_1");

            migrationBuilder.RenameColumn(
                name: "Iso_3166_1",
                table: "FilmReleases",
                newName: "Iso3166_1");

            migrationBuilder.RenameColumn(
                name: "Iso_3166_1",
                table: "Countries",
                newName: "Iso3166_1");

            migrationBuilder.RenameColumn(
                name: "Iso_3166_1",
                table: "AlternativeTitles",
                newName: "Iso3166_1");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "FilmReleases",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "FilmReleases");

            migrationBuilder.RenameColumn(
                name: "Iso639_1",
                table: "Languages",
                newName: "Iso_639_1");

            migrationBuilder.RenameColumn(
                name: "Iso3166_1",
                table: "FilmReleases",
                newName: "Iso_3166_1");

            migrationBuilder.RenameColumn(
                name: "Iso3166_1",
                table: "Countries",
                newName: "Iso_3166_1");

            migrationBuilder.RenameColumn(
                name: "Iso3166_1",
                table: "AlternativeTitles",
                newName: "Iso_3166_1");

            migrationBuilder.AddColumn<string>(
                name: "Awards",
                table: "Filme",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
