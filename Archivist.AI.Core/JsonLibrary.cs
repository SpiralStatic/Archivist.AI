using System.Text.Json;

namespace Archivist.AI.Core;

public class JsonLibrary : ILibrary
{
    private readonly FileStream _fileStream;

    public JsonLibrary(string jsonFilePath)
    {
        _fileStream = File.Open(jsonFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
    }

    public async Task<List<Embedding>> ReadLibrary()
    {
        var embeddings = new List<Embedding>();
        using StreamReader reader = new(_fileStream);
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
        using StreamWriter writer = new(_fileStream);

        foreach (var embedding in embeddings)
        {
            await writer.WriteLineAsync(JsonSerializer.Serialize(embedding));
        }
    }
}
