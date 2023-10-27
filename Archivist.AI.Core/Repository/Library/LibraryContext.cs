using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Archivist.AI.Core.Repository.Library;

public class LibraryContext : DbContext
{
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Archive> Archives { get; set; }
    public DbSet<Record> Records { get; set; }

    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<Dictionary<string, string>>()
            .HaveConversion<DictionaryConverter<string>>();

        configurationBuilder
            .Properties<ICollection<double>>()
            .HaveConversion<ListConverter<double>>();

        base.ConfigureConventions(configurationBuilder);
    }
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
    public required Dictionary<string, string> OwnershipProperties { get; set; }
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
    public Guid ArchiveId { get; set; }
    public required string Text { get; set; }
    public required DateTime WorldDate { get; set; }
    public required ICollection<double> EmbeddingValue { get; set; }
};

public class DictionaryConverter<TValue> : ValueConverter<Dictionary<string, TValue>, string>
{
    public DictionaryConverter() : base
        (
            x => JsonSerializer.Serialize(x, JsonSerializerOptions.Default),
            x => JsonSerializer.Deserialize<Dictionary<string, TValue>>(x, JsonSerializerOptions.Default) ?? new Dictionary<string, TValue>()
        )
    {
    }
}

public class ListConverter<TValue> : ValueConverter<ICollection<TValue>, string>
{
    public ListConverter() : base
        (
            x => JsonSerializer.Serialize(x, JsonSerializerOptions.Default),
            x => JsonSerializer.Deserialize<List<TValue>>(x, JsonSerializerOptions.Default) ?? new List<TValue>()
        )
    {
    }
}