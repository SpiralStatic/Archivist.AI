using Archivist.AI.Core;
using Microsoft.AspNetCore.Mvc;
using OpenAI.ObjectModels.RequestModels;

namespace Archivist.AI.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost(Name = "PostMessage")]
    public async Task<string> Post(string message)
    {
        var chatMessageResponse = await _chatService.GetChatResponse(message, Enumerable.Empty<ChatMessage>());

        return chatMessageResponse.Content;
    }
}
