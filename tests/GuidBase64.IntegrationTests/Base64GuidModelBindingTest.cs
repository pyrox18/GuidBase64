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

        public static IEnumerable<object[]> Base64GuidPairData =>
            new List<object[]>
            {
                new object[] { new Guid("00000000-0000-0000-0000-000000000000"), "AAAAAAAAAAAAAAAAAAAAAA" },
                new object[] { new Guid("c6a44c9f-763a-4524-8c0b-04c599f001a6"), "n0ykxjp2JEWMCwTFmfABpg" },
                new object[] { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), "_____________________w" }
            };

        [Theory]
        [MemberData(nameof(Base64GuidPairData))]
        public async Task CanBindBase64GuidAsync(Guid guid, string encodedBase64Guid)
        {
            var httpResponse = await _client.GetAsync($"/api/values/{encodedBase64Guid}");

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var resultGuid = new Guid(stringResponse);
            Assert.Equal(guid, resultGuid);
        }
    }
}
