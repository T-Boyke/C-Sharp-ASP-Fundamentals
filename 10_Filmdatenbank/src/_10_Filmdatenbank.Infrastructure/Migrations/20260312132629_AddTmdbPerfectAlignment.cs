using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _10_Filmdatenbank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTmdbPerfectAlignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Deathday",
                table: "Personen",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Personen",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Homepage",
                table: "Personen",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImdbId",
                table: "Personen",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Popularity",
                table: "Personen",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TmdbId",
                table: "Personen",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackdropUrl",
                table: "Filme",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Budget",
                table: "Filme",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Homepage",
                table: "Filme",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImdbId",
                table: "Filme",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalLanguage",
                table: "Filme",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalTitle",
                table: "Filme",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Popularity",
                table: "Filme",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Revenue",
                table: "Filme",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Filme",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TmdbId",
                table: "Filme",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "VoteAverage",
                table: "Filme",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VoteCount",
                table: "Filme",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deathday",
                table: "Personen");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Personen");

            migrationBuilder.DropColumn(
                name: "Homepage",
                table: "Personen");

            migrationBuilder.DropColumn(
                name: "ImdbId",
                table: "Personen");

            migrationBuilder.DropColumn(
                name: "Popularity",
                table: "Personen");

            migrationBuilder.DropColumn(
                name: "TmdbId",
                table: "Personen");

            migrationBuilder.DropColumn(
                name: "BackdropUrl",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "Budget",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "Homepage",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "ImdbId",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "OriginalLanguage",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "OriginalTitle",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "Popularity",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "Revenue",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "TmdbId",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "VoteAverage",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "VoteCount",
                table: "Filme");
        }
    }
}
