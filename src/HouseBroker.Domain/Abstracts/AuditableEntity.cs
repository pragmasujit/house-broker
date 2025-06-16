namespace HouseBroker.Domain.Abstracts;

public class AuditableEntity: EntityBase
{
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public string? CreatedBy { get; init; }
    public string? UpdatedBy { get; init; }
    
}