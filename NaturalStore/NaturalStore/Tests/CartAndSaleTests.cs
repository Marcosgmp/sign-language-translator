using Domain.Entities;
using Domain.Enums;
using Xunit;

namespace Tests;

public class ShoppingCartTests
{
    private static Product MakeProduct(string name = "Castanha", decimal stock = 20m) =>
        new Product(name, 10m, SaleUnit.Kilo, stock, new Category("Cat"));

    [Fact]
    public void AddItem_ShouldUpdateTotal()
    {
        var cart = new ShoppingCart(null, false);
        var p = MakeProduct();
        cart.AddItem(p, 2m);
        Assert.Equal(20m, cart.Total);
    }

    [Fact]
    public void AddSameProduct_ShouldAccumulateQuantity()
    {
        var cart = new ShoppingCart(null, false);
        var p = MakeProduct();
        cart.AddItem(p, 2m);
        cart.AddItem(p, 3m);
        Assert.Single(cart.Items);
        Assert.Equal(5m, cart.Items[0].Quantity);
    }

    [Fact]
    public void RemoveItem_ShouldEmptyCart()
    {
        var cart = new ShoppingCart(null, false);
        var p = MakeProduct();
        cart.AddItem(p, 1m);
        cart.RemoveItem(p.Id);
        Assert.True(cart.IsEmpty);
    }

    [Fact]
    public void Clear_ShouldEmptyCart()
    {
        var cart = new ShoppingCart(null, false);
        cart.AddItem(MakeProduct(), 2m);
        cart.Clear();
        Assert.True(cart.IsEmpty);
    }
}

public class SaleTests
{
    [Fact]
    public void CreateSale_ValidData_ShouldComputeTotal()
    {
        var items = new[] { new SaleItem("Produto A", 2m, 15m), new SaleItem("Produto B", 1m, 30m) };
        var sale  = new Sale(null, null, 1, false, PaymentMethod.Cash, items);
        Assert.Equal(60m, sale.Total);
    }

    [Fact]
    public void CreateSale_EmptyItems_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() =>
            new Sale(null, null, 1, false, PaymentMethod.Cash, Array.Empty<SaleItem>()));
    }

    [Fact]
    public void OnlineSale_WithCash_ShouldThrow()
    {
        var items = new[] { new SaleItem("X", 1m, 10m) };
        Assert.Throws<InvalidOperationException>(() =>
            new Sale(null, null, 1, true, PaymentMethod.Cash, items));
    }
}
