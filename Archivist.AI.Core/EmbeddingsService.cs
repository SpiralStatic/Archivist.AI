using Archivist.AI.Core.Repository.Library;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.Tokenizer.GPT3;

namespace Archivist.AI.Core;

public class EmbeddingsService : IEmbeddingsService
{
    private readonly IOpenAIService _openAIService;
    private readonly ILibrary _library;

    private List<Embedding> _embeddingsLibrary = new();

    public EmbeddingsService(IOpenAIService openAIService, ILibrary library)
    {
        _openAIService = openAIService;
        _library = library;
    }

    public async Task UpdateEmbeddings(string text)
    {
        var estimatedTokenCount = TokenizerGpt3.TokenCount(text);

        if (estimatedTokenCount > Models.EmbeddingModel.MaxTokens)
        {
            throw new ArchivistException(ArchivistException.EmbeddingMaxTokensLimit);
        }

        List<string> testEmbeddings = new()
        {
            "Hagar of clan Blackrook is a human barbarian who comes from clan Blackrook in the northern wastes of Gol-dressia",
            "Hagar wields a large lightning axe that was taken from the fae realm",
            "The young sleeping red dragon was charged by Hagar, forcing the team in to action",
            "The adventurer's party of Nememia and Hagar were joined by the elves Thea and Marni",
            "Thea is an elf that has trained in the arts of bladesinging, a fusion of blades and magic"
        };

        var requestInput = text.Trim() == "default" ? testEmbeddings : new List<string> { text };

        var embeddingResponse = await _openAIService.Embeddings.CreateEmbedding(new EmbeddingCreateRequest
        {
            Model = Models.EmbeddingModel.Model,
            InputAsList = requestInput
        });

        if (embeddingResponse.Successful && embeddingResponse?.Data != null)
        {
            var embeddings = requestInput.Zip(embeddingResponse.Data)
                .Select(x => new Embedding(Guid.NewGuid(), x.First, x.Second.Embedding))
                .ToList();

            await _library.UpdateLibrary(new Guid(), embeddings);
        }
        else
        {
            throw new ArchivistException(ArchivistException.EmbeddingBadResponse);
        }
    }

    public async Task<List<Embedding>> GetRelatedEmbeddings(string text)
    {
        var embeddingResponse = await _openAIService.Embeddings.CreateEmbedding(new EmbeddingCreateRequest
        {
            Model = Models.EmbeddingModel.Model,
            Input = text,
        });

        if (!embeddingResponse.Successful || embeddingResponse?.Data == null)
        {
            throw new ArchivistException(ArchivistException.EmbeddingBadResponse);
        }

        var e = embeddingResponse.Data.SelectMany(x => x.Embedding).ToArray();

        if (_embeddingsLibrary.Count == 0)
        {
            // TODO: Better sync
            _embeddingsLibrary = await _library.ReadLibrary(Guid.Empty);
        }

        var similarities = _embeddingsLibrary
            .Select(emb => (emb, DotProduct(emb.EmbeddingValue.ToArray(), e)))
            .OrderByDescending(x => x.Item2)
            .Take(10)
            .Select(x => x.emb)
            .ToList();

        return similarities;
    }

    private static double DotProduct(double[] vector1, double[] vector2)
    {
        return vector1.Zip(vector2).Sum(v => v.First * v.Second);
    }

    public async Task<Embedding> GetEmbedding(Guid id)
    {
        if (_embeddingsLibrary.Count == 0)
        {
            // TODO: Better sync
            _embeddingsLibrary = await _library.ReadLibrary(Guid.Empty);
        }

        return _embeddingsLibrary.First(x => x.Id == id);
    }

    public async Task<List<Embedding>> GetEmbeddings()
    {
        if (_embeddingsLibrary.Count == 0)
        {
            // TODO: Better sync
            _embeddingsLibrary = await _library.ReadLibrary(Guid.Empty);
        }

        return _embeddingsLibrary;
    }
}
