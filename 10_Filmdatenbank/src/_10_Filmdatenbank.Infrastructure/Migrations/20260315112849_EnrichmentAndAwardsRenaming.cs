using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _10_Filmdatenbank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EnrichmentAndAwardsRenaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TvdbId",
                table: "Filme",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TvdbId",
                table: "Filme");
        }
    }
}
