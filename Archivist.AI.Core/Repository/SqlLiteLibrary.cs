namespace Archivist.AI.Core.Repository;

public class SqlLiteLibrary : ILibrary
{
    private readonly LibraryContext _libraryContext;

    public SqlLiteLibrary(LibraryContext libraryContext)
    {
        _libraryContext = libraryContext;
    }

    public Task<List<Embedding>> ReadLibrary(Guid ownerId)
    {
        var result = _libraryContext.Archives
                        .First(x => x.OwnerId == ownerId)
                        .Records
                            .Select(x => new Embedding(x.Text, x.EmbeddingValue))
                            .ToList();

        return Task.FromResult(result);
    }

    public Task UpdateLibrary(List<Embedding> embeddings)
    {
        throw new NotImplementedException();
    }
}
