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
using _10_Filmdatenbank.Application.Interfaces;

namespace _10_Filmdatenbank.UnitTests.Application.Services
{
    public class MetacriticServiceTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<MetacriticService>> _loggerMock;
        private readonly MetacriticService _service;

        public MetacriticServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_handlerMock.Object);
            _loggerMock = new Mock<ILogger<MetacriticService>>();
            _service = new MetacriticService(_httpClient, _loggerMock.Object);
        }

        [Fact]
        public async Task GetDeepMetadataAsync_Should_Return_Null_When_Title_Is_Empty()
        {
            var result = await _service.GetDeepMetadataAsync("");
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetDeepMetadataAsync_Should_Return_Metadata_When_Html_Contains_Data()
        {
            // Arrange
            var title = "The Matrix";
            
            // Search response
            var searchHtml = @"<html><body>
                <script id=""__NEXT_DATA__"" type=""application/json"">
                {
                    ""props"": {
                        ""pageProps"": {
                            ""searchResults"": {
                                ""results"": [
                                    {
                                        ""title"": ""The Matrix"",
                                        ""releaseDate"": ""1999"",
                                        ""type"": ""movie"",
                                        ""slug"": ""the-matrix""
                                    }
                                ]
                            }
                        }
                    }
                }
                </script>
            </body></html>";

            // Movie detail response
            var movieHtml = @"<html><body>
                <script id=""__NEXT_DATA__"" type=""application/json"">
                {
                    ""props"": {
                        ""pageProps"": {
                            ""components"": [
                                {
                                    ""data"": {
                                        ""score"": 73,
                                        ""userScore"": 8.7
                                    }
                                }
                            ]
                        }
                    }
                }
                </script>
            </body></html>";

            _handlerMock.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(searchHtml)
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(movieHtml)
                });

            // Act
            var result = await _service.GetDeepMetadataAsync(title);

            // Assert
            result.Should().NotBeNull();
            result!.Metascore.Should().Be(73);
            result!.UserScore.Should().Be(8.7);
        }
    }
}
