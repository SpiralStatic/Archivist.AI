using OpenAI.ObjectModels.RequestModels;

namespace Archivist.AI.Core;

public interface IChatService
{
    Task<ChatMessage> GetChatResponse(string usersQuestion, IEnumerable<ChatMessage>? chatMessages);
}