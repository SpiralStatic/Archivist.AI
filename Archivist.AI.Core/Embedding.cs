namespace Archivist.AI.Core;

public record Embedding(Guid Id, string Text, IEnumerable<double> EmbeddingValue);