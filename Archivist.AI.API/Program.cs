using Archivist.AI.Core;
using Archivist.AI.Core.Repository.Library;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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

builder.Services.AddAuthorization(cfg =>
{
    cfg.AddPolicy("ChatPermission", policy => policy.RequireClaim("ownerid").RequireClaim("pchat", "true"));
    cfg.AddPolicy("StorytellerPermission", policy => policy.RequireClaim("ownerid").RequireClaim("pstry", "true"));
    cfg.AddPolicy("ManagementPermission", policy => policy.RequireClaim("ownerid").RequireClaim("pmngt", "true"));
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("Bearer",
       new OpenApiSecurityScheme
       {
           Description = "JWT Authorization header using the Bearer scheme.",
           Type = SecuritySchemeType.Http,
           Scheme = "Bearer"
       }
    );

    x.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            }, new List<string>()
        }
    });
});

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
