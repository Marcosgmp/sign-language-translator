using Domain.Enums;

namespace Domain.Entities;

public class SaleItem
{
    public string ProductName { get; }
    public decimal Quantity { get; }
    public decimal UnitPrice { get; }
    public decimal Subtotal => UnitPrice * Quantity;

    public SaleItem(string productName, decimal quantity, decimal unitPrice)
    {
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}

public class Sale : BaseEntity
{
    public DateTime Date { get; private set; }
    public Client? Client { get; private set; }
    public Employee? Employee { get; private set; }
    public int StoreId { get; private set; }
    public bool IsOnlineSale { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public IReadOnlyList<SaleItem> Items { get; private set; }
    public decimal Total { get; private set; }

    public Sale(Client? client, Employee? employee, int storeId, bool isOnlineSale,
                PaymentMethod paymentMethod, IEnumerable<SaleItem> items)
    {
        if (items == null || !items.Any()) throw new ArgumentException("Sale must have at least one item.");
        if (isOnlineSale && (paymentMethod == PaymentMethod.Cash))
            throw new InvalidOperationException("Online sales cannot be paid in cash.");

        Client = client;
        Employee = employee;
        StoreId = storeId;
        IsOnlineSale = isOnlineSale;
        PaymentMethod = paymentMethod;
        Items = items.ToList().AsReadOnly();
        Total = Items.Sum(i => i.Subtotal);
        Date = DateTime.Now;
    }
}
