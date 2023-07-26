using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;

namespace Archivist.AI.Core;

public class ChatService : IChatService
{
    private readonly IEmbeddingsService _embeddingsService;
    private readonly IOpenAIService _openAIService;

    private readonly List<ChatMessage> _chatHistory = new List<ChatMessage>();

    public ChatService(IEmbeddingsService embeddingsService, IOpenAIService openAIService)
    {
        _embeddingsService = embeddingsService;
        _openAIService = openAIService;
    }

    public async Task<string> GetChatResponse(string usersQuestion)
    {
        var relatedEmbeddings = await _embeddingsService.GetRelatedEmbeddings(usersQuestion);

        var predefinedInfo = relatedEmbeddings.Select(x => ChatMessage.FromSystem(x.Text));

        var messages = predefinedInfo
            .Prepend(ChatMessage.FromSystem("You are a storyteller"))
            .Concat(_chatHistory)
            .Append(ChatMessage.FromUser(usersQuestion))
            .ToList();

        var response = await _openAIService.ChatCompletion.CreateCompletion(
            new ChatCompletionCreateRequest
            {
                Model = Models.CompletionModel,
                Messages = messages
            }
        );

        if (response.Successful)
        {
            var message = response.Choices.First().Message;
            _chatHistory.Add(message);
            return message.Content;
        }

        throw new Exception();
    }
}
