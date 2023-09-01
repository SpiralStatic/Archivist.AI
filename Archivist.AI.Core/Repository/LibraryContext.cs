using Microsoft.EntityFrameworkCore;
using OpenAI.ObjectModels.ResponseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace Archivist.AI.Core.Repository;

public class LibraryContext : DbContext
{
    public DbSet<Archive> Archives { get; set; }
}

public abstract record DbModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime Inserted { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime LastUpdated { get; set; }
}

[Table(nameof(Owner))]
public record Owner : DbModel
{
    public Guid Id { get; set; }
    public IDictionary<string, string>? OwnershipProperties { get; set; }
    public ICollection<Archive>? Archives { get; }
}

[Table(nameof(Archive))]
public record Archive : DbModel
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public ICollection<Record> Records { get; } = null!;
}

[Table(nameof(Record))]
public record Record : DbModel
{
    public Guid Id { get; set; }
    public required string Text { get; set; }
    public required DateTime WorldDate { get; set; }
    public required EmbeddingResponse EmbeddingValue { get; set; }
};
