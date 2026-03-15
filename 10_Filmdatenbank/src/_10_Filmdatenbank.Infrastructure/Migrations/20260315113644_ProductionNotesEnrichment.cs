using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _10_Filmdatenbank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProductionNotesEnrichment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductionNotes",
                table: "Filme",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductionNotes",
                table: "Filme");
        }
    }
}
