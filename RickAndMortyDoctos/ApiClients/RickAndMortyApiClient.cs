using RickAndMortyDoctos.Models;
using System.Text.Json;

namespace RickAndMortyDoctos.ApiClients
{
    public class RickAndMortyService : IRickAndMortyApiClient
    {
        private readonly HttpClient _httpClient;

        public RickAndMortyService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<Episode>> GetAllEpisodesAsync()
        {
            var episodes = new List<Episode>();
            var nextUrl = "https://rickandmortyapi.com/api/episode";
            while (!string.IsNullOrEmpty(nextUrl))
            {
                var response = await _httpClient.GetAsync(nextUrl);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var pageResult = JsonSerializer.Deserialize<ApiResult<Episode>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                episodes.AddRange(pageResult.Results);
                nextUrl = pageResult.Info.Next;
            }
            return episodes;
        }

        public async Task<Character> GetCharacterAsync(string characterUrl)
        {
            var response = await _httpClient.GetAsync(characterUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var character = JsonSerializer.Deserialize<Character>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return character ?? throw new InvalidOperationException("Failed to deserialize the character.");
        }

        public class ApiResult<T>
        {
            public PageInfo Info { get; set; }
            public List<T> Results { get; set; }
        }

        public class PageInfo
        {
            public string Next { get; set; }
        }
    }
}
