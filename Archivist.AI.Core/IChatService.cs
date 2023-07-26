namespace Archivist.AI.Core;

public interface IChatService
{
    Task<string> GetChatResponse(string usersQuestion);
}