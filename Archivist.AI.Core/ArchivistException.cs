namespace Archivist.AI.Core;

public class ArchivistException : Exception
{
    public const string EmbeddingMaxTokensLimit = "Given embedding string is above the allowed max token limit";
    public const string ChatBadResponse = "Chat completion request gave non-successful response";
    public const string JsonLibraryFailedDeserialization = "Failed to deserialize the given json string";

    public ArchivistException()
    {
    }

    public ArchivistException(string message)
        : base(message)
    {
    }

    public ArchivistException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
