namespace Domain.Entities;

public abstract class BaseEntity
{
    private static int _counter = 0;
    public int Id { get; protected set; }

    protected BaseEntity()
    {
        Id = ++_counter;
    }
}
