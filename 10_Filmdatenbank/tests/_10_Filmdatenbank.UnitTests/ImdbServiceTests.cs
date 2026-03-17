using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using _10_Filmdatenbank.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Xunit;
using FluentAssertions;

namespace _10_Filmdatenbank.UnitTests.Application.Services
{
    public class ImdbServiceTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<ImdbService>> _loggerMock;
        private readonly ImdbService _service;

        public ImdbServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_handlerMock.Object);
            _loggerMock = new Mock<ILogger<ImdbService>>();
            _service = new ImdbService(_httpClient, _loggerMock.Object);
        }

        [Fact]
        public async Task GetMetadataAsync_Should_Return_Null_When_ImdbId_Is_Empty()
        {
            var result = await _service.GetMetadataAsync("");
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetMetadataAsync_Should_Return_Metadata_When_Html_Contains_LdJson()
        {
            // Arrange
            var imdbId = "tt1234567";
            var html = @"<html><body>
                <script type=""application/ld+json"">
                {
                    ""aggregateRating"": {
                        ""ratingValue"": 8.5
                    }
                }
                </script>
            </body></html>";

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(html)
                });

            // Act
            var result = await _service.GetMetadataAsync(imdbId);

            // Assert
            result.Should().NotBeNull();
            result!.Rating.Should().Be(8.5);
        }

        [Fact]
        public async Task GetMetadataAsync_Should_Return_Metadata_When_Html_Contains_NextData()
        {
            // Arrange
            var imdbId = "tt1234567";
            var html = @"<html><body>
                <script id=""__NEXT_DATA__"" type=""application/json"">
                {
                    ""props"": {
                        ""pageProps"": {
                            ""aboveTheFoldData"": {
                                ""metacritic"": {
                                    ""metascore"": {
                                        ""score"": 88
                                    }
                                },
                                ""ratingsSummary"": {
                                    ""aggregateRating"": 8.2
                                }
                            }
                        }
                    }
                }
                </script>
            </body></html>";

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(html)
                });

            // Act
            var result = await _service.GetMetadataAsync(imdbId);

            // Assert
            result.Should().NotBeNull();
            result!.Metascore.Should().Be(88);
            result!.Rating.Should().Be(8.2);
        }

        [Fact]
        public async Task GetMetadataAsync_Should_Return_Null_When_No_Metadata_Found()
        {
            // Arrange
            var imdbId = "tt1234567";
            var html = "<html><body>No data here</body></html>";

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(html)
                });

            // Act
            var result = await _service.GetMetadataAsync(imdbId);

            // Assert
            result.Should().BeNull();
        }
    }
}
