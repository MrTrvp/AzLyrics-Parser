using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using AzLyricParser.Core.Extensions;
using AzLyricParser.Core.Models;

namespace AzLyricParser.Core.Providers
{
    public class AzLyricProvider : IDisposable
    {
        private readonly HttpClient _client;

        public AzLyricProvider()
        {
            _client = new HttpClient(new HttpClientHandler
            {
                AllowAutoRedirect = false,
                UseProxy = false
            });
        }

        public async Task<SearchResult> Search(string query, int page = 0)
        {
            var queryCollection = new NameValueCollection
            {
                {"q", query},
                {"p", page.ToString()},
                {"w", "songs"}
            };

            var url = new Uri("http://search.azlyrics.com/search.php" + queryCollection.ToQueryString());
            return Helpers.AzLyricParser.ParseSearch(await _client.GetStringAsync(url), _client);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}