namespace HouseBroker.Domain.Abstracts;

public abstract class EntityBase
{
    public int Id { get; init; }
    public Guid Guid { get; init; }

    public EntityBase()
    {
        Guid = Guid.NewGuid();
    }
}