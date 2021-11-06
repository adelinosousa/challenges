using NSubstitute;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Pokedex.Application.UnitTests
{
    public class TestBase
    {
        protected static IHttpClientFactory httpClientFactory;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            httpClientFactory = Substitute.For<IHttpClientFactory>();
        }

        protected HttpClient GetMockHttpClient(HttpStatusCode httpStatusCode, object jsonContent = null)
        {
            return new HttpClient(new MockHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = httpStatusCode,
                Content = new StringContent(JsonSerializer.Serialize(jsonContent, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                }), Encoding.UTF8, "application/json")
            }));
        }
    }

    public class MockHttpMessageHandler : DelegatingHandler
    {
        private readonly HttpResponseMessage mockResponse;

        public MockHttpMessageHandler(HttpResponseMessage responseMessage)
        {
            mockResponse = responseMessage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(mockResponse);
        }
    }
}
