using Archivist.AI.Core;
using Microsoft.AspNetCore.Mvc;

namespace Archivist.AI.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly ILogger<ChatController> _logger;
    private readonly IChatService _chatService;

    public ChatController(ILogger<ChatController> logger, IChatService chatService)
    {
        _logger = logger;
        _chatService = chatService;
    }

    [HttpPost(Name = "PostMessage")]
    public async Task<string> Post(string message)
    {
        return await _chatService.GetChatResponse(message);
    }
}
