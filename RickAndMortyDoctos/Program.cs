using RickAndMortyDoctos.ApiClients;
using RickAndMortyDoctos.Models;

class Program
{
    static async Task Main(string[] args)
    {
        using (var httpClient = new HttpClient())
        {
            var service = new RickAndMortyService(httpClient);
            var characterAppearances = new Dictionary<string, int>();

            Console.WriteLine("Fetching episodes...");
            var episodes = await service.GetAllEpisodesAsync();

            foreach (var episode in episodes)
            {
                Console.WriteLine($"Episode: {episode.Name}");
                Console.WriteLine("Characters:");

                // Prepare a list to hold all character fetch tasks
                var characterFetchTasks = new List<Task<Character>>();

                // Queue up all tasks
                foreach (var characterUrl in episode.Characters)
                {
                    characterFetchTasks.Add(service.GetCharacterAsync(characterUrl));
                }

                // Wait for all tasks to complete
                var characters = await Task.WhenAll(characterFetchTasks);

                // Process each character
                foreach (var character in characters)
                {
                    Console.WriteLine($"- {character.Name}");

                    if (characterAppearances.ContainsKey(character.Name))
                    {
                        characterAppearances[character.Name]++;
                    }
                    else
                    {
                        characterAppearances[character.Name] = 1;
                    }
                }

                Console.WriteLine(new string('-', 40));
                Console.WriteLine();
            }

            Console.WriteLine("All episodes fetched.");
            PrintTenMostCharacterAppearances(characterAppearances);
        }
    }

    private static void PrintTenMostCharacterAppearances(Dictionary<string, int> characterAppearances)
    {
        var topCharacters = characterAppearances.OrderByDescending(kvp => kvp.Value).Take(10);

        Console.WriteLine("\nCharacter Appearances Graph:");
        int maxAppearances = topCharacters.Max(kvp => kvp.Value);
        foreach (var (name, count) in topCharacters)
        {
            int barLength = (count * 20) / maxAppearances;
            string bar = new string('█', barLength);
            Console.WriteLine($"{name.PadRight(20)}: {bar} {count}");
        }
    }
}
