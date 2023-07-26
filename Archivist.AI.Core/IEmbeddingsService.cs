namespace Archivist.AI.Core;

public interface IEmbeddingsService
{
    Task UpdateEmbeddings();

    Task<List<Embedding>> GetRelatedEmbeddings(string text);
}