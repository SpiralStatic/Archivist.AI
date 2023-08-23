using Archivist.AI.Core;
using Microsoft.AspNetCore.Mvc;

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
        return await _chatService.GetChatResponse(message);
    }
}
