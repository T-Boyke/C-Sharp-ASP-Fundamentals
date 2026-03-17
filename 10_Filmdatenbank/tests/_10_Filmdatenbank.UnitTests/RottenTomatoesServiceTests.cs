using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Xunit;
using FluentAssertions;
using _10_Filmdatenbank.Application.Services;
using _10_Filmdatenbank.Application.Models.RottenTomatoes;
using System;
using System.Collections.Generic;

namespace _10_Filmdatenbank.UnitTests.Services
{
    public class RottenTomatoesServiceTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<RottenTomatoesService>> _loggerMock;
        private readonly RottenTomatoesService _service;

        public RottenTomatoesServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_handlerMock.Object);
            _loggerMock = new Mock<ILogger<RottenTomatoesService>>();
            _service = new RottenTomatoesService(_httpClient, _loggerMock.Object);
        }

        [Fact]
        public async Task SearchMovieAsync_ReturnsNull_OnHttpError()
        {
            // Arrange
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));

            // Act
            var result = await _service.SearchMovieAsync("Inception");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task SearchMovieAsync_ReturnsHit_OnSuccessfulSearch()
        {
            // Arrange
            var responseData = new AlgoliaSearchResponse
            {
                Results = new List<AlgoliaResult>
                {
                    new AlgoliaResult
                    {
                        Hits = new List<RottenTomatoesHit>
                        {
                            new RottenTomatoesHit
                            {
                                Title = "Inception",
                                Type = "movie",
                                ReleaseYear = 2010,
                                Scores = new RottenTomatoesScores { CriticsScore = 87, AudienceScore = 91 }
                            }
                        }
                    }
                }
            };

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(responseData)
            };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.SearchMovieAsync("Inception", 2010);

            // Assert
            result.Should().NotBeNull();
            result!.Title.Should().Be("Inception");
            result.Scores!.CriticsScore.Should().Be(87);
        }

        [Fact]
        public async Task SearchMovieAsync_ReturnsNull_WhenNoHitsFound()
        {
            // Arrange
            var responseData = new AlgoliaSearchResponse { Results = new List<AlgoliaResult>() };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(responseData)
            };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.SearchMovieAsync("NonExistentMovie");

            // Assert
            result.Should().BeNull();
        }
    }
}
