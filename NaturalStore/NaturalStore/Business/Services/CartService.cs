using Domain.Entities;
using Domain.Interfaces;

namespace Business.Services;

public class CartService
{
    private readonly IProductRepository _productRepo;

    public CartService(IProductRepository productRepo) => _productRepo = productRepo;

    public ShoppingCart CreateCart(Client? client, bool isOnline) => new ShoppingCart(client, isOnline);

    public void AddItem(ShoppingCart cart, int productId, decimal quantity)
    {
        var product = _productRepo.GetById(productId)
            ?? throw new InvalidOperationException($"Product {productId} not found.");
        if (product.StockQuantity < quantity)
            throw new InvalidOperationException($"Insufficient stock for '{product.Name}'.");
        cart.AddItem(product, quantity);
    }

    public void RemoveItem(ShoppingCart cart, int productId) => cart.RemoveItem(productId);

    public void UpdateQuantity(ShoppingCart cart, int productId, decimal quantity)
    {
        var product = _productRepo.GetById(productId)
            ?? throw new InvalidOperationException($"Product {productId} not found.");
        if (product.StockQuantity < quantity)
            throw new InvalidOperationException($"Insufficient stock for '{product.Name}'.");
        cart.UpdateItemQuantity(productId, quantity);
    }

    public void ClearCart(ShoppingCart cart) => cart.Clear();
}
