using Archivist.AI.Core;
using Microsoft.Extensions.Hosting;
using OpenAI.ObjectModels.RequestModels;

namespace Archivist.AI.Console;

public class ArchivistHostedService : IHostedService
{
    private readonly IEmbeddingsService _embeddingsService;
    private readonly IChatService _chatService;

    public ArchivistHostedService(IEmbeddingsService embeddingsService, IChatService chatService)
    {
        _embeddingsService = embeddingsService;
        _chatService = chatService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        System.Console.WriteLine("Starting archivist...");

        //await _embeddingsService.UpdateEmbeddings();

        while (!cancellationToken.IsCancellationRequested)
        {
            var usersQuestion = System.Console.ReadLine();

            if (string.IsNullOrWhiteSpace(usersQuestion))
            {
                continue;
            }

            var response = await _chatService.GetChatResponse(usersQuestion, Enumerable.Empty<ChatMessage>());

            System.Console.WriteLine(response);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
