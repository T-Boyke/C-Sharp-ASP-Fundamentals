using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _10_Filmdatenbank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRatingsFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ImdbRating",
                table: "Filme",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RottenTomatoesAudienceRating",
                table: "Filme",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RottenTomatoesCriticRating",
                table: "Filme",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TvdbRating",
                table: "Filme",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImdbRating",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "RottenTomatoesAudienceRating",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "RottenTomatoesCriticRating",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "TvdbRating",
                table: "Filme");
        }
    }
}
