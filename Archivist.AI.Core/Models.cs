namespace Archivist.AI.Core;

public static class Models
{
    public static readonly (string Model, int MaxTokens) EmbeddingModel = ("text-embedding-ada-002", 8191);

    public const string CompletionModel = "gpt-3.5-turbo";
}
