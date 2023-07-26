using Archivist.AI.Console;
using Archivist.AI.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenAI.Extensions;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddOpenAIService();
        services.AddSingleton<IChatService, ChatService>();
        services.AddSingleton<IEmbeddingsService, EmbeddingsService>();
        services.AddSingleton<ILibrary, JsonLibrary>((x) => new JsonLibrary("library.jsonl"));
        services.AddHostedService<ArchivistHostedService>();
    })
    .Build()
    .RunAsync();
