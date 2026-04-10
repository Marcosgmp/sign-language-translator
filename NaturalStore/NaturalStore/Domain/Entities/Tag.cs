namespace Domain.Entities;

public class Tag : BaseEntity
{
    public string Name { get; private set; }

    public Tag(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Tag name cannot be empty.");
        Name = name.Trim();
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Tag name cannot be empty.");
        Name = name.Trim();
    }
}
