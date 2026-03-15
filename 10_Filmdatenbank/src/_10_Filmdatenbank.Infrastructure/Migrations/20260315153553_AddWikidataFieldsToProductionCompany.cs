using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _10_Filmdatenbank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWikidataFieldsToProductionCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeCount",
                table: "ProductionCompanies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FoundedYear",
                table: "ProductionCompanies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WikidataId",
                table: "ProductionCompanies",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeCount",
                table: "ProductionCompanies");

            migrationBuilder.DropColumn(
                name: "FoundedYear",
                table: "ProductionCompanies");

            migrationBuilder.DropColumn(
                name: "WikidataId",
                table: "ProductionCompanies");
        }
    }
}
