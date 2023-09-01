using Archivist.AI.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OpenAI.ObjectModels.RequestModels;

namespace Archivist.AI.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly IMemoryCache _memoryCache;

    public ChatController(IChatService chatService, IMemoryCache memoryCache)
    {
        _chatService = chatService;
        _memoryCache = memoryCache;
    }

    [HttpPost(Name = "PostMessage")]
    public async Task<IActionResult> Post(string message)
    {
        var ownerId = User.Claims.FirstOrDefault(x => x.Type == "OwnerId")?.Value;

        if (ownerId == null)
        {
            return Unauthorized("Invalid OwnerId");
        }

        var chatHistory = GetChatHistory(ownerId);

        var chatMessageResponse = await _chatService.GetChatResponse(message, chatHistory);

        _memoryCache.Set(ownerId, chatMessageResponse);

        return Ok(chatMessageResponse.Content);

        IEnumerable<ChatMessage> GetChatHistory(string? ownerId)
        {
            if (ownerId != null && _memoryCache.TryGetValue(ownerId, out object? value) &&
                        value is IEnumerable<ChatMessage> chatMessages)
            {
                return chatMessages;
            }
            else
            {
                return Enumerable.Empty<ChatMessage>();
            }
        }
    }
}
