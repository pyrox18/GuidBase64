using GuidBase64.CommonTestData;
using GuidBase64.TestWebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GuidBase64.IntegrationTests
{
    public class Base64GuidModelBindingTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public Base64GuidModelBindingTest(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        public static IEnumerable<object[]> Base64GuidPairData => TestData.Base64GuidPairs;

        [Theory]
        [MemberData(nameof(Base64GuidPairData))]
        public async Task CanBindBase64Guid(Guid guid, string encodedBase64Guid)
        {
            var httpResponse = await _client.GetAsync($"/api/values/{encodedBase64Guid}");

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var resultGuid = new Guid(stringResponse);
            Assert.Equal(guid, resultGuid);
        }
    }
}
