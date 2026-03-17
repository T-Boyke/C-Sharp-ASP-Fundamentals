using System;
using System.Collections.Generic;
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
    public class WikidataServiceTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<WikidataService>> _loggerMock;
        private readonly WikidataService _service;

        public WikidataServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_handlerMock.Object);
            _loggerMock = new Mock<ILogger<WikidataService>>();
            _service = new WikidataService(_httpClient, _loggerMock.Object);
        }

        [Fact]
        public async Task GetPersonDetailsAsync_Should_Return_Null_When_Ids_Are_Empty()
        {
            var result = await _service.GetPersonDetailsAsync(null, null);
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetPersonDetailsAsync_Should_Return_Details_When_Valid_Id_Is_Provided()
        {
            // Arrange
            var wikidataId = "Q42";
            var jsonResponse = @"{
                ""results"": {
                    ""bindings"": [
                        {
                            ""birthPlaceLabel"": { ""value"": ""London"" },
                            ""zodiacLabel"": { ""value"": ""Aries"" },
                            ""insta"": { ""value"": ""insta_id"" },
                            ""twitter"": { ""value"": ""twitter_id"" },
                            ""fb"": { ""value"": ""fb_id"" },
                            ""desc"": { ""value"": ""A famous person."" }
                        }
                    ]
                }
            }";

            // Mocking ExecuteSparqlAsync for both details and awards (2 calls)
            _handlerMock.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(@"{ ""results"": { ""bindings"": [] } }") // Empty awards
                });

            // Act
            var result = await _service.GetPersonDetailsAsync(wikidataId);

            // Assert
            result.Should().NotBeNull();
            result!.BirthPlace.Should().Be("London");
            result!.ZodiacSign.Should().Be("Aries");
            result!.InstagramId.Should().Be("insta_id");
            result!.TwitterId.Should().Be("twitter_id");
            result!.FacebookId.Should().Be("fb_id");
            result!.Description.Should().Be("A famous person.");
        }
    }
}
