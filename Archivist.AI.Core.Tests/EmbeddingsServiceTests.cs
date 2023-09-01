using Archivist.AI.Core.Repository;
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

    #region UpdateEmbeddings
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
        _openAIService.Embeddings.CreateEmbedding(default!)
            .ReturnsForAnyArgs(Task.FromResult(new EmbeddingCreateResponse { Error = new Error() }));

        const string text = "test";

        await _sut.UpdateEmbeddings(text).ShouldThrowAsync<ArchivistException>();
    }

    [Test]
    public async Task UpdateEmbeddings_GivenEmbeddingApiResponseWithNoData_ThrowsArchivistException()
    {
        _openAIService.Embeddings.CreateEmbedding(default!)
            .ReturnsForAnyArgs(Task.FromResult(new EmbeddingCreateResponse()));

        const string text = "test";

        await _sut.UpdateEmbeddings(text).ShouldThrowAsync<ArchivistException>();
    }

    [Test]
    public async Task UpdateEmbeddings_GivenEmbeddingApiSuccessfulResponse_Returns()
    {
        _openAIService.Embeddings.CreateEmbedding(default!)
            .ReturnsForAnyArgs(Task.FromResult(new EmbeddingCreateResponse { Data = new List<EmbeddingResponse>() }));

        const string text = "test";

        await _sut.UpdateEmbeddings(text).ShouldNotThrowAsync();
    }
    #endregion

    #region GetRelatedEmbeddings
    [Test]
    public async Task GetRelatedEmbeddings_GivenEmbeddingApiBadResponse_ThrowsArchivistException()
    {
        _openAIService.Embeddings.CreateEmbedding(default!)
            .ReturnsForAnyArgs(Task.FromResult(new EmbeddingCreateResponse { Error = new Error() }));

        const string text = "test";

        await _sut.GetRelatedEmbeddings(text).ShouldThrowAsync<ArchivistException>();
    }

    [Test]
    public void GetRelatedEmbeddings_GivenMatchingEmbeddings_ReturnsEmbeddings()
    {
        var givenEmbedding = new EmbeddingResponse { Embedding = new List<double> { 1, 2, 3 } };
        var existingEmbedding = new EmbeddingResponse { Embedding = new List<double> { 1, 2, 3 } };
        var existingEmbedding2 = new EmbeddingResponse { Embedding = new List<double> { 1, 2, 3 } };

        _openAIService.Embeddings.CreateEmbedding(default!)
            .ReturnsForAnyArgs(Task.FromResult(new EmbeddingCreateResponse { Data = new List<EmbeddingResponse> { givenEmbedding } }));

        _library.ReadLibrary()
            .Returns(Task.FromResult(new List<Embedding> { new Embedding("test2", existingEmbedding), new Embedding("test3", existingEmbedding2) }));

        const string text = "test";

        _sut.GetRelatedEmbeddings(text)
            .Result
            .ShouldBe(new List<Embedding> {
                new Embedding("test2", existingEmbedding),
                new Embedding("test3", existingEmbedding2)
            });
    }
    #endregion
}