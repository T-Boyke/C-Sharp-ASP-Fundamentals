using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _10_Filmdatenbank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModelConsistencyFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteFilms_Filme_FilmID",
                table: "FavoriteFilms");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteFilms_Filme_FilmID",
                table: "FavoriteFilms",
                column: "FilmID",
                principalTable: "Filme",
                principalColumn: "FilmID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteFilms_Filme_FilmID",
                table: "FavoriteFilms");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteFilms_Filme_FilmID",
                table: "FavoriteFilms",
                column: "FilmID",
                principalTable: "Filme",
                principalColumn: "FilmID");
        }
    }
}
