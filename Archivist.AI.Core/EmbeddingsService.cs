﻿using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;

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

    public async Task UpdateEmbeddings()
    {
        // todo: token limit of 8191
        List<string> test = new()
        {
                "Hagar of clan Blackrook is a human barbarian who comes from clan Blackrook in the northern wastes of Gol-dressia",
                "Hagar wields a large lightning axe that was taken from the fae realm",
                "The young sleeping red dragon was charged by Hagar, forcing the team in to action",
                "The adventurer's party of Nememia and Hagar were joined by the elves Thea and Marni",
                "Thea is an elf that has trained in the arts of bladesinging, a fusion of blades and magic"
            };

        var embeddingResponse = await _openAIService.Embeddings.CreateEmbedding(new EmbeddingCreateRequest
        {
            Model = Models.EmbeddingModel,
            InputAsList = test
        });

        if (embeddingResponse.Successful)
        {
            var embeddings = test.Zip(embeddingResponse.Data).Select(x => new Embedding(x.First, x.Second)).ToList();
            await _library.UpdateLibrary(embeddings);
        }
    }

    public async Task<List<Embedding>> GetRelatedEmbeddings(string text)
    {
        var embeddingResponse = await _openAIService.Embeddings.CreateEmbedding(new EmbeddingCreateRequest
        {
            Model = Models.EmbeddingModel,
            Input = text,
        });

        var e = embeddingResponse.Data.SelectMany(x => x.Embedding).ToArray();

        if (_embeddingsLibrary.Count == 0)
        {
            // TODO: Better sync
            _embeddingsLibrary = await _library.ReadLibrary();
        }

        var similarities = _embeddingsLibrary
            .Select(emb => (emb, DotProduct(emb.EmbeddingValue.Embedding.ToArray(), e)))
            .OrderByDescending(x => x.Item2)
            .Take(5)
            .Select(x => x.emb)
            .ToList();

        return similarities;
    }

    private static double DotProduct(double[] vector1, double[] vector2)
    {
        return vector1.Zip(vector2).Sum(v => v.First * v.Second);
    }
}
