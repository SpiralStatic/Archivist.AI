namespace Archivist.AI.Core.Repository;

public interface ILibrary
{
    Task<List<Embedding>> ReadLibrary(Guid ownerId);

    Task UpdateLibrary(List<Embedding> embeddings);
}