using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _10_Filmdatenbank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FanaticDataExpansion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Runtime",
                table: "Filme",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FilmAwards",
                columns: table => new
                {
                    AwardID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    IsWin = table.Column<bool>(type: "bit", nullable: false),
                    FilmID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmAwards", x => x.AwardID);
                    table.ForeignKey(
                        name: "FK_FilmAwards_Filme_FilmID",
                        column: x => x.FilmID,
                        principalTable: "Filme",
                        principalColumn: "FilmID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonAwards",
                columns: table => new
                {
                    AwardID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    IsWin = table.Column<bool>(type: "bit", nullable: false),
                    PersonID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonAwards", x => x.AwardID);
                    table.ForeignKey(
                        name: "FK_PersonAwards_Personen_PersonID",
                        column: x => x.PersonID,
                        principalTable: "Personen",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionCompanyAwards",
                columns: table => new
                {
                    AwardID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    IsWin = table.Column<bool>(type: "bit", nullable: false),
                    ProductionCompanyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionCompanyAwards", x => x.AwardID);
                    table.ForeignKey(
                        name: "FK_ProductionCompanyAwards_ProductionCompanies_ProductionCompanyID",
                        column: x => x.ProductionCompanyID,
                        principalTable: "ProductionCompanies",
                        principalColumn: "ProductionCompanyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilmAwards_FilmID",
                table: "FilmAwards",
                column: "FilmID");

            migrationBuilder.CreateIndex(
                name: "IX_FilmAwards_Name",
                table: "FilmAwards",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PersonAwards_Name",
                table: "PersonAwards",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PersonAwards_PersonID",
                table: "PersonAwards",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionCompanyAwards_Name",
                table: "ProductionCompanyAwards",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionCompanyAwards_ProductionCompanyID",
                table: "ProductionCompanyAwards",
                column: "ProductionCompanyID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmAwards");

            migrationBuilder.DropTable(
                name: "PersonAwards");

            migrationBuilder.DropTable(
                name: "ProductionCompanyAwards");

            migrationBuilder.DropColumn(
                name: "Runtime",
                table: "Filme");
        }
    }
}
