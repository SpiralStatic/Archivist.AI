using OpenAI.ObjectModels.ResponseModels;

namespace Archivist.AI.Core;

public record Embedding(string Text, List<double> EmbeddingValue);