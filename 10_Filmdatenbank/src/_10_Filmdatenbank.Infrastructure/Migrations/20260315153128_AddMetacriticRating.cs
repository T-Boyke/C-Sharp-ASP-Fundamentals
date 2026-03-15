using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _10_Filmdatenbank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMetacriticRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MetacriticRating",
                table: "Filme",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetacriticRating",
                table: "Filme");
        }
    }
}
