using NSubstitute;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.ResponseModels;
using Shouldly;

namespace Archivist.AI.Core.Tests;

public class EmbeddingsServiceTests
{
    private EmbeddingsService _sut;
    private readonly IOpenAIService _openAIService = Substitute.For<IOpenAIService>();
    private readonly ILibrary _library = Substitute.For<ILibrary>();

    [SetUp]
    public void Setup()
    {
        _sut = new EmbeddingsService(_openAIService, _library);
    }

    [Test]
    public async Task UpdateEmbeddings_GivenTextAboveMaxTokenLimit_ThrowsArchivistException()
    {
        var veryLongText = Enumerable.Range(0, 850)
            .Select(_ => "Lorem ipsum dolor sit amet")
            .Aggregate((agg, cur) => agg + cur);

        await _sut.UpdateEmbeddings(veryLongText).ShouldThrowAsync<ArchivistException>();
    }

    [Test]
    public async Task UpdateEmbeddings_GivenEmbeddingApiBadResponse_ThrowsArchivistException()
    {
        _openAIService.Embeddings.CreateEmbedding(default)
            .ReturnsForAnyArgs(Task.FromResult(new EmbeddingCreateResponse { Error = new Error() }));

        const string text = "test";

        await _sut.UpdateEmbeddings(text).ShouldThrowAsync<ArchivistException>();
    }
}