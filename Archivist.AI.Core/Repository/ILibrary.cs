namespace Archivist.AI.Core.Repository;

public interface ILibrary
{
    Task<List<Embedding>> ReadLibrary();

    Task UpdateLibrary(List<Embedding> embeddings);
}