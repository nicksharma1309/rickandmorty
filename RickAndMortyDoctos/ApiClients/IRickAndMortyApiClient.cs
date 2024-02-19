using RickAndMortyDoctos.Models;

namespace RickAndMortyDoctos.ApiClients
{
    public interface IRickAndMortyApiClient
    {
        Task<IEnumerable<Episode>> GetAllEpisodesAsync();
        Task<Character> GetCharacterAsync(string characterUrl);
    }
}
