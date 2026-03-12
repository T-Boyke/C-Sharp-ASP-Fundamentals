using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _10_Filmdatenbank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    CollectionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TmdbId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Overview = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PosterUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackdropUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.CollectionID);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Iso_3166_1 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NativeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Iso_3166_1);
                });

            migrationBuilder.CreateTable(
                name: "Eigenschaften",
                columns: table => new
                {
                    EigenschaftID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bezeichnung = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eigenschaften", x => x.EigenschaftID);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    GenreID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TmdbId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.GenreID);
                });

            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    KeywordID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TmdbId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.KeywordID);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Iso_639_1 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NativeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Iso_639_1);
                });

            migrationBuilder.CreateTable(
                name: "Personen",
                columns: table => new
                {
                    PersonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Vorname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nachname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Biografie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilBildUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Geburtsdatum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Geburtsort = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TmdbId = table.Column<int>(type: "int", nullable: true),
                    ImdbId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    Deathday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Homepage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Popularity = table.Column<double>(type: "float", nullable: true),
                    Awards = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KnownForDepartment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlsoKnownAs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adult = table.Column<bool>(type: "bit", nullable: false),
                    WikidataId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacebookId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstagramId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwitterId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FreebaseId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FreebaseMid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TvrageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TmdbFilmographyJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personen", x => x.PersonID);
                });

            migrationBuilder.CreateTable(
                name: "ProductionCompanies",
                columns: table => new
                {
                    CompanyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TmdbId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Headquarters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Homepage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentCompanyID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionCompanies", x => x.CompanyID);
                    table.ForeignKey(
                        name: "FK_ProductionCompanies_ProductionCompanies_ParentCompanyID",
                        column: x => x.ParentCompanyID,
                        principalTable: "ProductionCompanies",
                        principalColumn: "CompanyID");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Filme",
                columns: table => new
                {
                    FilmID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Erscheinungsjahr = table.Column<int>(type: "int", nullable: false),
                    Spieldauer = table.Column<int>(type: "int", nullable: false),
                    Preis = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Handlung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tagline = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PosterUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Erscheinungsdatum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Nutzerwertung = table.Column<double>(type: "float", nullable: true),
                    TmdbId = table.Column<int>(type: "int", nullable: true),
                    ImdbId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalLanguage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Budget = table.Column<long>(type: "bigint", nullable: true),
                    Revenue = table.Column<long>(type: "bigint", nullable: true),
                    Homepage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Popularity = table.Column<double>(type: "float", nullable: true),
                    VoteCount = table.Column<int>(type: "int", nullable: true),
                    VoteAverage = table.Column<double>(type: "float", nullable: true),
                    BackdropUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WikidataId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacebookId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstagramId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwitterId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrailerUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Awards = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adult = table.Column<bool>(type: "bit", nullable: false),
                    CollectionID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filme", x => x.FilmID);
                    table.ForeignKey(
                        name: "FK_Filme_Collections_CollectionID",
                        column: x => x.CollectionID,
                        principalTable: "Collections",
                        principalColumn: "CollectionID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AlternativeTitles",
                columns: table => new
                {
                    AlternativeTitleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilmID = table.Column<int>(type: "int", nullable: false),
                    Iso_3166_1 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlternativeTitles", x => x.AlternativeTitleID);
                    table.ForeignKey(
                        name: "FK_AlternativeTitles_Filme_FilmID",
                        column: x => x.FilmID,
                        principalTable: "Filme",
                        principalColumn: "FilmID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmGenres",
                columns: table => new
                {
                    FilmID = table.Column<int>(type: "int", nullable: false),
                    GenreID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmGenres", x => new { x.FilmID, x.GenreID });
                    table.ForeignKey(
                        name: "FK_FilmGenres_Filme_FilmID",
                        column: x => x.FilmID,
                        principalTable: "Filme",
                        principalColumn: "FilmID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmGenres_Genres_GenreID",
                        column: x => x.GenreID,
                        principalTable: "Genres",
                        principalColumn: "GenreID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmKeywords",
                columns: table => new
                {
                    FilmID = table.Column<int>(type: "int", nullable: false),
                    KeywordID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmKeywords", x => new { x.FilmID, x.KeywordID });
                    table.ForeignKey(
                        name: "FK_FilmKeywords_Filme_FilmID",
                        column: x => x.FilmID,
                        principalTable: "Filme",
                        principalColumn: "FilmID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmKeywords_Keywords_KeywordID",
                        column: x => x.KeywordID,
                        principalTable: "Keywords",
                        principalColumn: "KeywordID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmProductionCompanies",
                columns: table => new
                {
                    CompanyID = table.Column<int>(type: "int", nullable: false),
                    FilmID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmProductionCompanies", x => new { x.CompanyID, x.FilmID });
                    table.ForeignKey(
                        name: "FK_FilmProductionCompanies_Filme_FilmID",
                        column: x => x.FilmID,
                        principalTable: "Filme",
                        principalColumn: "FilmID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmProductionCompanies_ProductionCompanies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "ProductionCompanies",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmProductionCountries_ISO",
                columns: table => new
                {
                    FilmID = table.Column<int>(type: "int", nullable: false),
                    Iso_3166_1 = table.Column<string>(type: "nvarchar(2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmProductionCountries_ISO", x => new { x.FilmID, x.Iso_3166_1 });
                    table.ForeignKey(
                        name: "FK_FilmProductionCountries_ISO_Countries_Iso_3166_1",
                        column: x => x.Iso_3166_1,
                        principalTable: "Countries",
                        principalColumn: "Iso_3166_1",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmProductionCountries_ISO_Filme_FilmID",
                        column: x => x.FilmID,
                        principalTable: "Filme",
                        principalColumn: "FilmID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmRecommended",
                columns: table => new
                {
                    FilmID = table.Column<int>(type: "int", nullable: false),
                    RecommendedFilmID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmRecommended", x => new { x.FilmID, x.RecommendedFilmID });
                    table.ForeignKey(
                        name: "FK_FilmRecommended_Filme_FilmID",
                        column: x => x.FilmID,
                        principalTable: "Filme",
                        principalColumn: "FilmID");
                    table.ForeignKey(
                        name: "FK_FilmRecommended_Filme_RecommendedFilmID",
                        column: x => x.RecommendedFilmID,
                        principalTable: "Filme",
                        principalColumn: "FilmID");
                });

            migrationBuilder.CreateTable(
                name: "FilmReleases",
                columns: table => new
                {
                    ReleaseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilmID = table.Column<int>(type: "int", nullable: false),
                    Iso_3166_1 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Certification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmReleases", x => x.ReleaseID);
                    table.ForeignKey(
                        name: "FK_FilmReleases_Filme_FilmID",
                        column: x => x.FilmID,
                        principalTable: "Filme",
                        principalColumn: "FilmID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmSimilar",
                columns: table => new
                {
                    FilmID = table.Column<int>(type: "int", nullable: false),
                    SimilarFilmID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmSimilar", x => new { x.FilmID, x.SimilarFilmID });
                    table.ForeignKey(
                        name: "FK_FilmSimilar_Filme_FilmID",
                        column: x => x.FilmID,
                        principalTable: "Filme",
                        principalColumn: "FilmID");
                    table.ForeignKey(
                        name: "FK_FilmSimilar_Filme_SimilarFilmID",
                        column: x => x.SimilarFilmID,
                        principalTable: "Filme",
                        principalColumn: "FilmID");
                });

            migrationBuilder.CreateTable(
                name: "FilmSpokenLanguages",
                columns: table => new
                {
                    FilmID = table.Column<int>(type: "int", nullable: false),
                    Iso_639_1 = table.Column<string>(type: "nvarchar(2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmSpokenLanguages", x => new { x.FilmID, x.Iso_639_1 });
                    table.ForeignKey(
                        name: "FK_FilmSpokenLanguages_Filme_FilmID",
                        column: x => x.FilmID,
                        principalTable: "Filme",
                        principalColumn: "FilmID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmSpokenLanguages_Languages_Iso_639_1",
                        column: x => x.Iso_639_1,
                        principalTable: "Languages",
                        principalColumn: "Iso_639_1",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonEigenschaftFilme",
                columns: table => new
                {
                    PEFID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonID = table.Column<int>(type: "int", nullable: false),
                    FilmID = table.Column<int>(type: "int", nullable: false),
                    EigenschaftID = table.Column<int>(type: "int", nullable: false),
                    Job = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Character = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    CreditId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonEigenschaftFilme", x => x.PEFID);
                    table.ForeignKey(
                        name: "FK_PersonEigenschaftFilme_Eigenschaften_EigenschaftID",
                        column: x => x.EigenschaftID,
                        principalTable: "Eigenschaften",
                        principalColumn: "EigenschaftID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonEigenschaftFilme_Filme_FilmID",
                        column: x => x.FilmID,
                        principalTable: "Filme",
                        principalColumn: "FilmID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonEigenschaftFilme_Personen_PersonID",
                        column: x => x.PersonID,
                        principalTable: "Personen",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlternativeTitles_FilmID",
                table: "AlternativeTitles",
                column: "FilmID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_TmdbId",
                table: "Collections",
                column: "TmdbId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Filme_CollectionID",
                table: "Filme",
                column: "CollectionID");

            migrationBuilder.CreateIndex(
                name: "IX_FilmGenres_GenreID",
                table: "FilmGenres",
                column: "GenreID");

            migrationBuilder.CreateIndex(
                name: "IX_FilmKeywords_KeywordID",
                table: "FilmKeywords",
                column: "KeywordID");

            migrationBuilder.CreateIndex(
                name: "IX_FilmProductionCompanies_FilmID",
                table: "FilmProductionCompanies",
                column: "FilmID");

            migrationBuilder.CreateIndex(
                name: "IX_FilmProductionCountries_ISO_Iso_3166_1",
                table: "FilmProductionCountries_ISO",
                column: "Iso_3166_1");

            migrationBuilder.CreateIndex(
                name: "IX_FilmRecommended_RecommendedFilmID",
                table: "FilmRecommended",
                column: "RecommendedFilmID");

            migrationBuilder.CreateIndex(
                name: "IX_FilmReleases_FilmID",
                table: "FilmReleases",
                column: "FilmID");

            migrationBuilder.CreateIndex(
                name: "IX_FilmSimilar_SimilarFilmID",
                table: "FilmSimilar",
                column: "SimilarFilmID");

            migrationBuilder.CreateIndex(
                name: "IX_FilmSpokenLanguages_Iso_639_1",
                table: "FilmSpokenLanguages",
                column: "Iso_639_1");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_TmdbId",
                table: "Genres",
                column: "TmdbId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_TmdbId",
                table: "Keywords",
                column: "TmdbId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonEigenschaftFilme_EigenschaftID",
                table: "PersonEigenschaftFilme",
                column: "EigenschaftID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonEigenschaftFilme_FilmID",
                table: "PersonEigenschaftFilme",
                column: "FilmID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonEigenschaftFilme_PersonID",
                table: "PersonEigenschaftFilme",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionCompanies_ParentCompanyID",
                table: "ProductionCompanies",
                column: "ParentCompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionCompanies_TmdbId",
                table: "ProductionCompanies",
                column: "TmdbId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlternativeTitles");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "FilmGenres");

            migrationBuilder.DropTable(
                name: "FilmKeywords");

            migrationBuilder.DropTable(
                name: "FilmProductionCompanies");

            migrationBuilder.DropTable(
                name: "FilmProductionCountries_ISO");

            migrationBuilder.DropTable(
                name: "FilmRecommended");

            migrationBuilder.DropTable(
                name: "FilmReleases");

            migrationBuilder.DropTable(
                name: "FilmSimilar");

            migrationBuilder.DropTable(
                name: "FilmSpokenLanguages");

            migrationBuilder.DropTable(
                name: "PersonEigenschaftFilme");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "ProductionCompanies");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Eigenschaften");

            migrationBuilder.DropTable(
                name: "Filme");

            migrationBuilder.DropTable(
                name: "Personen");

            migrationBuilder.DropTable(
                name: "Collections");
        }
    }
}
