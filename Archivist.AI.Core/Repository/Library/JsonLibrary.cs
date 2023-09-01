using System.Text.Json;

namespace Archivist.AI.Core.Repository.Library;

public class JsonLibrary : ILibrary
{
    private readonly string _jsonFilePath;

    public JsonLibrary(string jsonFilePath)
    {
        _jsonFilePath = jsonFilePath;
    }

    public async Task<List<Embedding>> ReadLibrary(Guid ownerId)
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
                    throw new ArchivistException(ArchivistException.JsonLibraryFailedDeserialization, ex);
                }
            }
        }

        return embeddings;
    }

    public async Task UpdateLibrary(Guid archiveId, List<Embedding> embeddings)
    {
        var fileStream = File.Open(_jsonFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        using StreamWriter writer = new(fileStream);

        foreach (var embedding in embeddings)
        {
            await writer.WriteLineAsync(JsonSerializer.Serialize(embedding));
        }
    }
}
