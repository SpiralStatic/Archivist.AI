namespace Archivist.AI.Core.Repository.Library;

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
                            .Select(x => new Embedding(x.Id, x.Text, x.EmbeddingValue))
                            .ToList();

        return Task.FromResult(result);
    }

    public async Task UpdateLibrary(Guid archiveId, List<Embedding> embeddings)
    {
        var records = embeddings.Select(x =>
        {
            return new Record
            {
                ArchiveId = archiveId,
                Text = x.Text,
                EmbeddingValue = x.EmbeddingValue.ToList(),
                WorldDate = DateTime.UtcNow
            };
        });

        await _libraryContext.Records.AddRangeAsync(records);
    }
}
