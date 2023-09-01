namespace Archivist.AI.Core.Repository;

public interface ILibrary
{
    Task<List<Embedding>> ReadLibrary(Guid ownerId);

    Task UpdateLibrary(Guid archiveId, List<Embedding> embeddings);
}