using Domain.Entities;
using Domain.Enums;
using Xunit;

namespace Tests;

public class ProductTests
{
    private static Category MakeCategory() => new Category("Oleaginosas");

    private static Product MakeProduct(decimal stock = 10m) =>
        new Product("Castanha", 45m, SaleUnit.Kilo, stock, MakeCategory());

    [Fact]
    public void CreateProduct_ValidData_ShouldSucceed()
    {
        var p = MakeProduct();
        Assert.Equal("Castanha", p.Name);
        Assert.Equal(45m, p.Price);
        Assert.Equal(10m, p.StockQuantity);
    }

    [Fact]
    public void CreateProduct_NegativePrice_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() =>
            new Product("X", -1m, SaleUnit.Unit, 10m, MakeCategory()));
    }

    [Fact]
    public void ReduceStock_ValidQuantity_ShouldDecrement()
    {
        var p = MakeProduct(20m);
        p.ReduceStock(5m);
        Assert.Equal(15m, p.StockQuantity);
    }

    [Fact]
    public void ReduceStock_Insufficient_ShouldThrow()
    {
        var p = MakeProduct(5m);
        Assert.Throws<InvalidOperationException>(() => p.ReduceStock(10m));
    }

    [Fact]
    public void AddTag_ShouldBeIncludedInList()
    {
        var p = MakeProduct();
        var tag = new Tag("Orgânico");
        p.AddTag(tag);
        Assert.Contains(tag, p.Tags);
    }

    [Fact]
    public void AddDuplicateTag_ShouldNotDuplicate()
    {
        var p = MakeProduct();
        var tag = new Tag("Vegano");
        p.AddTag(tag);
        p.AddTag(tag);
        Assert.Single(p.Tags);
    }
}
