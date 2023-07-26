using System.IO;
using System.Text.Json;

namespace Archivist.AI.Core;

public class JsonLibrary : ILibrary
{
    private string _jsonFilePath;

    public JsonLibrary(string jsonFilePath)
    {
        _jsonFilePath = jsonFilePath;
    }

    public async Task<List<Embedding>> ReadLibrary()
    {
        var fileStream = File.Open(_jsonFilePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
        var embeddings = new List<Embedding>();

        using StreamReader reader = new(fileStream);
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (line != null)
            {
                try
                {
                    var embedding = JsonSerializer.Deserialize<Embedding>(line);
                    if (embedding?.Text != null && embedding?.EmbeddingValue != null)
                    {
                        embeddings.Add(embedding);
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        return embeddings;
    }

    public async Task UpdateLibrary(List<Embedding> embeddings)
    {
        var fileStream = File.Open(_jsonFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        using StreamWriter writer = new(fileStream);

        foreach (var embedding in embeddings)
        {
            await writer.WriteLineAsync(JsonSerializer.Serialize(embedding));
        }
    }
}
