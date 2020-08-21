using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AlphaVantage.Net.TestUtils;
using FluentAssertions;
using Xunit;

namespace AlphaVantage.Net.Core.Tests
{
    public class TypedAlphaVantageClientShould
    {
        private readonly string _apiKey = ConfigProvider.Configuration["ApiKey"];
        
        [Fact]
        public async Task ReturnTypedResult()
        {
            var function = ApiFunction.TIME_SERIES_INTRADAY;
            var symbol = "AAPL";
            var interval = "15min";
            var query = new Dictionary<string, string>()
            {
                {nameof(symbol), symbol},
                {nameof(interval), interval}
            };

            using var client = new Client.AlphaVantageClient(_apiKey);
            using var typedClient = new Client.TypedAlphaVantageClient(client);
            
            var typedResponse = await typedClient.RequestApiAsync(new TestParser(), function, query);

            typedResponse.Should().BeOfType<string>().And.Be("Meta Data");
        }
        
        private class TestParser : IAlphaVantageParser<string>
        {
            public string ParseApiResponse(JsonDocument jsonDocument)
            {
                return jsonDocument.RootElement.EnumerateObject().FirstOrDefault().Name;
            }
        }
    }
}