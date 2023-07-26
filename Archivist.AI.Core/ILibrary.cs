using OpenAI.ObjectModels.ResponseModels;

namespace Archivist.AI.Core;

public interface ILibrary
{
    Task<List<Embedding>> ReadLibrary();

    Task UpdateLibrary(List<Embedding> embeddings);
}