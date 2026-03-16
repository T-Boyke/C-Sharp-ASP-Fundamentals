using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _10_Filmdatenbank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMetacriticDeepFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetacriticUrl",
                table: "Filme",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MetacriticUserScore",
                table: "Filme",
                type: "float",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MetacriticReviews",
                columns: table => new
                {
                    MetacriticReviewID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilmID = table.Column<int>(type: "int", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Publication = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Score = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetacriticReviews", x => x.MetacriticReviewID);
                    table.ForeignKey(
                        name: "FK_MetacriticReviews_Filme_FilmID",
                        column: x => x.FilmID,
                        principalTable: "Filme",
                        principalColumn: "FilmID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MetacriticReviews_FilmID",
                table: "MetacriticReviews",
                column: "FilmID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MetacriticReviews");

            migrationBuilder.DropColumn(
                name: "MetacriticUrl",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "MetacriticUserScore",
                table: "Filme");
        }
    }
}
