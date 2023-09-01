using NSubstitute;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;
using OpenAI.ObjectModels.SharedModels;
using Shouldly;
using static OpenAI.ObjectModels.StaticValues;

namespace Archivist.AI.Core.Tests;

public class ChatServiceTests
{
    private ChatService _sut;
    private readonly IOpenAIService _openAIService = Substitute.For<IOpenAIService>();
    private readonly IEmbeddingsService _embeddingsService = Substitute.For<IEmbeddingsService>();

    [SetUp]
    public void Setup()
    {
        _sut = new ChatService(_embeddingsService, _openAIService);
    }

    #region GetChatResponse
    [Test]
    public async Task GetChatResponse_GivenChatCompletionApiBadResponse_ThrowsArchivistException()
    {
        _embeddingsService.GetRelatedEmbeddings(default!)
            .ReturnsForAnyArgs(new List<Embedding>());

        _openAIService.ChatCompletion.CreateCompletion(default!)
            .ReturnsForAnyArgs(new ChatCompletionCreateResponse { Error = new Error() });

        const string text = "test";

        await _sut.GetChatResponse(text, Enumerable.Empty<ChatMessage>()).ShouldThrowAsync<ArchivistException>();
    }

    [Test]
    public async Task GetChatResponse_GivenChatCompletionApiSuccessfulResponse_Returns()
    {
        _embeddingsService.GetRelatedEmbeddings(default!)
            .ReturnsForAnyArgs(new List<Embedding>());

        _openAIService.ChatCompletion.CreateCompletion(default!)
            .ReturnsForAnyArgs(new ChatCompletionCreateResponse
            {
                Choices = new List<ChatChoiceResponse>
                {
                    new ChatChoiceResponse { Message = new ChatMessage(ChatMessageRoles.Assistant, "hello there") }
                }
            });

        const string text = "test";

        var result = await _sut.GetChatResponse(text, Enumerable.Empty<ChatMessage>());
        result.Content.ShouldBe("hello there");
    }
    #endregion
}