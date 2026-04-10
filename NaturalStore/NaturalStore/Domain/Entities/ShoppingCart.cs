using Domain.Enums;

namespace Domain.Entities;

public class CartItem
{
    public Product Product { get; }
    public decimal Quantity { get; private set; }
    public decimal Subtotal => Product.Price * Quantity;

    public CartItem(Product product, decimal quantity)
    {
        if (product is null) throw new ArgumentNullException(nameof(product));
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
        Product = product;
        Quantity = quantity;
    }

    public void UpdateQuantity(decimal quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
        Quantity = quantity;
    }
}

public class ShoppingCart : BaseEntity
{
    public Client? Client { get; private set; }
    public bool IsOnlineCart { get; private set; }
    private readonly List<CartItem> _items = new();
    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();
    public decimal Total => _items.Sum(i => i.Subtotal);

    public ShoppingCart(Client? client, bool isOnlineCart)
    {
        Client = client;
        IsOnlineCart = isOnlineCart;
    }

    public void AddItem(Product product, decimal quantity)
    {
        if (product is null) throw new ArgumentNullException(nameof(product));
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");

        var existing = _items.FirstOrDefault(i => i.Product.Id == product.Id);
        if (existing != null)
            existing.UpdateQuantity(existing.Quantity + quantity);
        else
            _items.Add(new CartItem(product, quantity));
    }

    public void RemoveItem(int productId)
    {
        var item = _items.FirstOrDefault(i => i.Product.Id == productId);
        if (item is null) throw new InvalidOperationException("Product not found in cart.");
        _items.Remove(item);
    }

    public void UpdateItemQuantity(int productId, decimal quantity)
    {
        var item = _items.FirstOrDefault(i => i.Product.Id == productId);
        if (item is null) throw new InvalidOperationException("Product not found in cart.");
        item.UpdateQuantity(quantity);
    }

    public void Clear() => _items.Clear();

    public bool IsEmpty => !_items.Any();
}
