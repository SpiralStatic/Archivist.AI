namespace Archivist.AI.Core;

public interface IEmbeddingsService
{
    Task UpdateEmbeddings(string text);

    Task<List<Embedding>> GetRelatedEmbeddings(string text);
}