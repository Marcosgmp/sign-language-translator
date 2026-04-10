using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Business.Services;

public class SaleService
{
    private readonly ISaleRepository _saleRepo;
    private readonly IProductRepository _productRepo;

    public SaleService(ISaleRepository saleRepo, IProductRepository productRepo)
    {
        _saleRepo = saleRepo;
        _productRepo = productRepo;
    }

    public Sale ProcessSale(ShoppingCart cart, Employee? employee, int storeId,
                            bool isOnline, PaymentMethod paymentMethod)
    {
        if (cart.IsEmpty)
            throw new InvalidOperationException("Cannot process an empty cart.");
        if (isOnline && cart.Client == null)
            throw new InvalidOperationException("Online sale requires a registered client.");
        if (!isOnline && employee == null)
            throw new InvalidOperationException("In-store sale requires an employee.");
        if (!isOnline && employee != null && !employee.HasRole(EmployeeRole.Cashier))
            throw new InvalidOperationException("Only cashiers can process in-store sales.");

        // Deduct stock
        foreach (var item in cart.Items)
        {
            var product = _productRepo.GetById(item.Product.Id)!;
            product.ReduceStock(item.Quantity);
        }

        var saleItems = cart.Items.Select(i => new SaleItem(i.Product.Name, i.Quantity, i.Product.Price));
        var sale = new Sale(cart.Client, employee, storeId, isOnline, paymentMethod, saleItems);
        _saleRepo.Add(sale);

        cart.Client?.AddPurchase(sale);
        cart.Clear();
        return sale;
    }

    public IEnumerable<Sale> GetAll() => _saleRepo.GetAll();
    public IEnumerable<Sale> GetByStore(int storeId) => _saleRepo.GetByStore(storeId);
    public IEnumerable<Sale> GetByClient(int clientId) => _saleRepo.GetByClient(clientId);

    public SalesReport GetReport(int year, int? month = null)
    {
        var sales = month.HasValue
            ? _saleRepo.GetByYearAndMonth(year, month.Value)
            : _saleRepo.GetByYear(year);

        return new SalesReport(sales.ToList(), year, month);
    }
}

public class SalesReport
{
    public int Year { get; }
    public int? Month { get; }
    public IReadOnlyList<Sale> Sales { get; }
    public decimal TotalRevenue { get; }
    public int TotalTransactions { get; }
    public decimal AverageTicket => TotalTransactions == 0 ? 0 : TotalRevenue / TotalTransactions;

    public SalesReport(IReadOnlyList<Sale> sales, int year, int? month)
    {
        Sales = sales;
        Year = year;
        Month = month;
        TotalRevenue = sales.Sum(s => s.Total);
        TotalTransactions = sales.Count;
    }
}
