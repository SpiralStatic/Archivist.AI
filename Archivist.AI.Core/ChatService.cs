using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;

namespace Archivist.AI.Core;

public class ChatService : IChatService
{
    private const string ChatRole = "Use the provided sentences delimited by triple quotes to help answer questions. You are a storyteller, but don't make things up";
    
    private readonly IEmbeddingsService _embeddingsService;
    private readonly IOpenAIService _openAIService;

    public ChatService(IEmbeddingsService embeddingsService, IOpenAIService openAIService)
    {
        _embeddingsService = embeddingsService;
        _openAIService = openAIService;
    }

    public async Task<ChatMessage> GetChatResponse(string usersQuestion, IEnumerable<ChatMessage>? chatHistory)
    {
        var relatedEmbeddings = await _embeddingsService.GetRelatedEmbeddings(usersQuestion);

        var predefinedInfo = relatedEmbeddings.Select(x => ChatMessage.FromSystem($"\"\"\"{x.Text}\"\"\""));

        var messages = predefinedInfo
            .Prepend(ChatMessage.FromSystem(ChatRole))
            .Concat(chatHistory ?? Enumerable.Empty<ChatMessage>())
            .Append(ChatMessage.FromUser(usersQuestion))
            .ToList();

        var response = await _openAIService.ChatCompletion.CreateCompletion(
            new ChatCompletionCreateRequest
            {
                Model = Models.CompletionModel,
                Temperature = 0.3f,
                PresencePenalty = 0.8f,
                Messages = messages
            }
        );

        if (response.Successful)
        {
            return response.Choices.First().Message;
        }

        throw new ArchivistException(ArchivistException.ChatBadResponse);
    }
}
