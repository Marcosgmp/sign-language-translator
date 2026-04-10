using Domain.Enums;

namespace Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public SaleUnit Unit { get; private set; }
    public decimal StockQuantity { get; private set; }
    public Category Category { get; private set; }
    private readonly List<Tag> _tags = new();
    public IReadOnlyList<Tag> Tags => _tags.AsReadOnly();

    public Product(string name, decimal price, SaleUnit unit, decimal stockQuantity, Category category)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Product name cannot be empty.");
        if (price < 0) throw new ArgumentException("Price cannot be negative.");
        if (stockQuantity < 0) throw new ArgumentException("Stock cannot be negative.");
        if (category is null) throw new ArgumentNullException(nameof(category));

        Name = name.Trim();
        Price = price;
        Unit = unit;
        StockQuantity = stockQuantity;
        Category = category;
    }

    public void Update(string name, decimal price, SaleUnit unit, decimal stockQuantity, Category category)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Product name cannot be empty.");
        if (price < 0) throw new ArgumentException("Price cannot be negative.");
        if (stockQuantity < 0) throw new ArgumentException("Stock cannot be negative.");
        if (category is null) throw new ArgumentNullException(nameof(category));

        Name = name.Trim();
        Price = price;
        Unit = unit;
        StockQuantity = stockQuantity;
        Category = category;
    }

    public void AddTag(Tag tag)
    {
        if (tag is null) throw new ArgumentNullException(nameof(tag));
        if (!_tags.Any(t => t.Id == tag.Id))
            _tags.Add(tag);
    }

    public void RemoveTag(int tagId) => _tags.RemoveAll(t => t.Id == tagId);

    public void ReduceStock(decimal quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
        if (StockQuantity < quantity) throw new InvalidOperationException($"Insufficient stock for '{Name}'.");
        StockQuantity -= quantity;
    }

    public void AddStock(decimal quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
        StockQuantity += quantity;
    }
}
