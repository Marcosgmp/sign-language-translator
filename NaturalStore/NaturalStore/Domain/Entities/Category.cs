namespace Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    public Category(string name, string description = "")
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Category name cannot be empty.");
        Name = name.Trim();
        Description = description.Trim();
    }

    public void Update(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Category name cannot be empty.");
        Name = name.Trim();
        Description = description.Trim();
    }
}
