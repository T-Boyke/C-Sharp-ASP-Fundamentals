using Xunit;
using _10_Filmdatenbank.Domain.Entities;
using FluentAssertions;
using System.Collections.Generic;
using System;

namespace _10_Filmdatenbank.UnitTests.Domain
{
    public class AdditionalEntityTests
    {
        [Fact]
        public void Genre_Properties_Work()
        {
            var genre = new Genre { GenreID = 1, Name = "Sci-Fi", TmdbId = 123, Films = new List<Film>() };
            genre.GenreID.Should().Be(1);
            genre.Name.Should().Be("Sci-Fi");
            genre.TmdbId.Should().Be(123);
            genre.Films.Should().NotBeNull();
        }

        [Fact]
        public void Keyword_Properties_Work()
        {
            var keyword = new Keyword { KeywordID = 1, Name = "Time Travel", TmdbId = 456, Films = new List<Film>() };
            keyword.KeywordID.Should().Be(1);
            keyword.Name.Should().Be("Time Travel");
            keyword.TmdbId.Should().Be(456);
            keyword.Films.Should().NotBeNull();
        }

        [Fact]
        public void ProductionCompany_Properties_Work()
        {
            var company = new ProductionCompany { ProductionCompanyID = 1, Name = "Warner Bros", Films = new List<Film>() };
            company.ProductionCompanyID.Should().Be(1);
            company.Name.Should().Be("Warner Bros");
            company.Films.Should().NotBeNull();
        }

        [Fact]
        public void Collection_Properties_Work()
        {
            var collection = new Collection { CollectionID = 1, Name = "Marvel Cinematic Universe", Films = new List<Film>() };
            collection.CollectionID.Should().Be(1);
            collection.Name.Should().Be("Marvel Cinematic Universe");
            collection.Films.Should().NotBeNull();
        }

        [Fact]
        public void Country_Properties_Work()
        {
            var country = new Country { Iso3166_1 = "DE", Name = "Germany", ProductionFilms = new List<Film>() };
            country.Iso3166_1.Should().Be("DE");
            country.Name.Should().Be("Germany");
            country.ProductionFilms.Should().NotBeNull();
        }

        [Fact]
        public void Language_Properties_Work()
        {
            var language = new Language { Iso639_1 = "de", Name = "German", SpokenInFilms = new List<Film>() };
            language.Iso639_1.Should().Be("de");
            language.Name.Should().Be("German");
            language.SpokenInFilms.Should().NotBeNull();
        }

        [Fact]
        public void BoxOfficeEntry_Properties_Work()
        {
            var entry = new BoxOfficeEntry { BoxOfficeEntryID = 1, Revenue = 1000000, Date = DateTime.Now };
            entry.BoxOfficeEntryID.Should().Be(1);
            entry.Revenue.Should().Be(1000000);
        }
    }
}
