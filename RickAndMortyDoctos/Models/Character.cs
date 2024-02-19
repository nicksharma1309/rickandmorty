using System.Text.Json.Serialization;

namespace RickAndMortyDoctos.Models
{
    public class Character
    {
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
