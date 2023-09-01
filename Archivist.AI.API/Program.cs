using Archivist.AI.Core;
using Archivist.AI.Core.Repository;
using OpenAI.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenAIService();
builder.Services.AddSingleton<IChatService, ChatService>();
builder.Services.AddSingleton<IEmbeddingsService, EmbeddingsService>();

// TODO: Better file access
var jsonLibraryPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new ArgumentException("Library location not found"), "Repository/library.jsonl");
builder.Services.AddSingleton<ILibrary, JsonLibrary>((x) => new JsonLibrary(jsonLibraryPath));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
