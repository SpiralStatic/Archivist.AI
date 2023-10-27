using Archivist.AI.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Archivist.AI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmbeddingsController : ControllerBase
{
    private readonly IEmbeddingsService _embeddingService;

    public EmbeddingsController(IEmbeddingsService embeddingService)
    {
        _embeddingService = embeddingService;
    }

    [HttpGet("{id}", Name = "GetEmbedding")]
    [Authorize(Policy = "ManagementPermission")]
    public async Task Get(Guid id)
    {
        await _embeddingService.GetEmbedding(id);
    }

    [HttpGet(Name = "GetEmbeddings")]
    [Authorize(Policy = "ManagementPermission")]
    public async Task GetAll()
    {
        await _embeddingService.GetEmbeddings();
    }

    [HttpPost(Name = "PostEmbedding")]
    [Authorize(Policy = "StorytellerPermission")]
    public async Task Post(string message)
    {
        await _embeddingService.UpdateEmbeddings(message);
    }
}
