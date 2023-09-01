namespace Archivist.AI.Core;

public record Embedding(string Text, IEnumerable<double> EmbeddingValue);