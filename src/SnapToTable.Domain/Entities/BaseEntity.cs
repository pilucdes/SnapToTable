namespace SnapToTable.Domain.Entities;

public record BaseEntity
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}