using Archivist.AI.Core;
using Archivist.AI.Core.Repository.Library;
using Microsoft.EntityFrameworkCore;
using OpenAI.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenAIService();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IEmbeddingsService, EmbeddingsService>();

// TODO: Better file access
var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
//var jsonLibraryPath = Path.Combine(currentDirectory ?? throw new ArgumentException("Library location not found"), "Repository/library.jsonl");
//builder.Services.AddSingleton<ILibrary, JsonLibrary>((x) => new JsonLibrary(jsonLibraryPath));

builder.Services.AddDbContext<LibraryContext>(options => options.UseSqlite($"Data Source={currentDirectory}/Repository/Library.db", b => b.MigrationsAssembly("Archivist.AI.API")));
builder.Services.AddScoped<ILibrary, SqlLiteLibrary>();

builder.Services.AddMemoryCache();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ChatPermission", policy => policy.RequireClaim("OwnerId"));
    options.AddPolicy("ManagementPermission", policy => policy.RequireClaim("OwnerId"));
});

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
