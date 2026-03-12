using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _10_Filmdatenbank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTmdbEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilmProductionCompanies_ProductionCompanies_CompanyID",
                table: "FilmProductionCompanies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FilmProductionCompanies",
                table: "FilmProductionCompanies");

            migrationBuilder.DropIndex(
                name: "IX_FilmProductionCompanies_FilmID",
                table: "FilmProductionCompanies");

            migrationBuilder.RenameColumn(
                name: "LogoPath",
                table: "ProductionCompanies",
                newName: "LogoUrl");

            migrationBuilder.RenameColumn(
                name: "CompanyID",
                table: "ProductionCompanies",
                newName: "ProductionCompanyID");

            migrationBuilder.RenameColumn(
                name: "CompanyID",
                table: "FilmProductionCompanies",
                newName: "ProductionCompanyID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FilmProductionCompanies",
                table: "FilmProductionCompanies",
                columns: new[] { "FilmID", "ProductionCompanyID" });

            migrationBuilder.CreateIndex(
                name: "IX_FilmProductionCompanies_ProductionCompanyID",
                table: "FilmProductionCompanies",
                column: "ProductionCompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_FilmProductionCompanies_ProductionCompanies_ProductionCompanyID",
                table: "FilmProductionCompanies",
                column: "ProductionCompanyID",
                principalTable: "ProductionCompanies",
                principalColumn: "ProductionCompanyID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilmProductionCompanies_ProductionCompanies_ProductionCompanyID",
                table: "FilmProductionCompanies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FilmProductionCompanies",
                table: "FilmProductionCompanies");

            migrationBuilder.DropIndex(
                name: "IX_FilmProductionCompanies_ProductionCompanyID",
                table: "FilmProductionCompanies");

            migrationBuilder.RenameColumn(
                name: "LogoUrl",
                table: "ProductionCompanies",
                newName: "LogoPath");

            migrationBuilder.RenameColumn(
                name: "ProductionCompanyID",
                table: "ProductionCompanies",
                newName: "CompanyID");

            migrationBuilder.RenameColumn(
                name: "ProductionCompanyID",
                table: "FilmProductionCompanies",
                newName: "CompanyID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FilmProductionCompanies",
                table: "FilmProductionCompanies",
                columns: new[] { "CompanyID", "FilmID" });

            migrationBuilder.CreateIndex(
                name: "IX_FilmProductionCompanies_FilmID",
                table: "FilmProductionCompanies",
                column: "FilmID");

            migrationBuilder.AddForeignKey(
                name: "FK_FilmProductionCompanies_ProductionCompanies_CompanyID",
                table: "FilmProductionCompanies",
                column: "CompanyID",
                principalTable: "ProductionCompanies",
                principalColumn: "CompanyID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
