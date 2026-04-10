using Business.Services;
using ConsoleApp.Helpers;
using Domain.Entities;
using Domain.Enums;
using BizContext = Business.AppContext;

namespace ConsoleApp.Menus;

public class SaleMenu
{
    private readonly BizContext _ctx;

    public SaleMenu(BizContext ctx) => _ctx = ctx;

    public void Show()
    {
        while (true)
        {
            int opt = UI.Menu("VENDAS",
                new[] { "Venda Presencial (PDV)", "Venda Online", "Listar vendas", "Relatório Gerencial" });
            switch (opt)
            {
                case 1: PhysicalSale(); break;
                case 2: OnlineSale(); break;
                case 3: ListSales(); break;
                case 4: Report(); break;
                case 0: return;
            }
        }
    }

    // ── PHYSICAL SALE ──────────────────────────────────────────────────────────
    private void PhysicalSale()
    {
        UI.Header("VENDA PRESENCIAL");

        // Select store
        var stores = _ctx.StoreService.GetAll().ToList();
        foreach (var s in stores) Console.WriteLine($"  [{s.Id}] {s.Name}");
        int storeId = UI.ReadInt("Loja (ID)");

        // Select cashier
        var cashiers = _ctx.EmployeeService.GetByRole(EmployeeRole.Cashier)
            .Where(e => e.StoreId == storeId).ToList();
        if (!cashiers.Any()) { UI.Error("Nenhum caixa disponível nesta loja."); UI.Pause(); return; }
        foreach (var e in cashiers) Console.WriteLine($"  [{e.Id}] {e.Name}");
        int empId = UI.ReadInt("Funcionário caixa (ID)");
        Employee? cashier;
        try { cashier = _ctx.EmployeeService.GetById(empId); }
        catch { UI.Error("Funcionário não encontrado."); UI.Pause(); return; }

        // Optional client
        Client? client = null;
        if (UI.Confirm("Cliente cadastrado?"))
        {
            int cid = UI.ReadInt("ID do cliente");
            try { client = _ctx.ClientService.GetById(cid); }
            catch { UI.Error("Cliente não encontrado. Prosseguindo sem vínculo."); }
        }

        var cart = _ctx.CartService.CreateCart(client, false);
        FillCart(cart, false);
        if (cart.IsEmpty) { UI.Info("Carrinho vazio. Venda cancelada."); UI.Pause(); return; }

        ShowCart(cart);

        Console.WriteLine("  Pagamento: [1] Crédito  [2] Débito  [3] Dinheiro");
        var payment = UI.ReadInt("Opção", 1, 3) switch
        {
            1 => PaymentMethod.CreditCard,
            2 => PaymentMethod.DebitCard,
            _ => PaymentMethod.Cash
        };

        try
        {
            var sale = _ctx.SaleService.ProcessSale(cart, cashier, storeId, false, payment);
            UI.Success($"Venda #{sale.Id} realizada! Total: R$ {sale.Total:F2}");
        }
        catch (Exception ex) { UI.Error(ex.Message); }
        UI.Pause();
    }

    // ── ONLINE SALE ────────────────────────────────────────────────────────────
    private void OnlineSale()
    {
        UI.Header("VENDA ONLINE");
        string login = UI.ReadRequired("Login");
        string pass  = UI.ReadRequired("Senha");

        var client = _ctx.ClientService.Login(login, pass);
        if (client == null) { UI.Error("Credenciais inválidas."); UI.Pause(); return; }
        UI.Success($"Bem-vindo, {client.Name}!");

        var cart = _ctx.CartService.CreateCart(client, true);
        FillCart(cart, true);
        if (cart.IsEmpty) { UI.Info("Carrinho vazio. Venda cancelada."); UI.Pause(); return; }

        ShowCart(cart);

        Console.WriteLine("  Pagamento: [1] Crédito  [2] Débito");
        var payment = UI.ReadInt("Opção", 1, 2) == 1 ? PaymentMethod.CreditCard : PaymentMethod.DebitCard;

        var stores = _ctx.StoreService.GetAll().ToList();
        int storeId = stores.First().Id; // online sales linked to first store

        try
        {
            var sale = _ctx.SaleService.ProcessSale(cart, null, storeId, true, payment);
            UI.Success($"Pedido #{sale.Id} confirmado! Total: R$ {sale.Total:F2}");
        }
        catch (Exception ex) { UI.Error(ex.Message); }
        UI.Pause();
    }

    // ── LIST ───────────────────────────────────────────────────────────────────
    private void ListSales()
    {
        UI.Header("VENDAS REALIZADAS");
        var sales = _ctx.SaleService.GetAll().ToList();
        if (!sales.Any()) { UI.Info("Nenhuma venda registrada."); UI.Pause(); return; }
        foreach (var s in sales)
        {
            string type   = s.IsOnlineSale ? "Online" : "Presencial";
            string client = s.Client?.Name ?? "Anônimo";
            Console.WriteLine($"  [{s.Id}] {s.Date:dd/MM/yyyy HH:mm} | {type} | Cliente: {client} | R$ {s.Total:F2} | {s.PaymentMethod}");
        }
        UI.Pause();
    }

    // ── REPORT ─────────────────────────────────────────────────────────────────
    private void Report()
    {
        UI.Header("RELATÓRIO DE VENDAS");
        int year = UI.ReadInt("Ano", 2020, DateTime.Now.Year);
        Console.WriteLine("  Filtrar por mês? (0 = ano inteiro)");
        Console.Write("  Mês (0-12): ");
        int.TryParse(Console.ReadLine(), out int month);

        var report = _ctx.SaleService.GetReport(year, month == 0 ? null : month);

        UI.Separator();
        Console.WriteLine($"  Período: {(month == 0 ? year.ToString() : $"{month:D2}/{year}")}");
        Console.WriteLine($"  Total de Vendas:  {report.TotalTransactions}");
        Console.WriteLine($"  Receita Total:    R$ {report.TotalRevenue:F2}");
        Console.WriteLine($"  Ticket Médio:     R$ {report.AverageTicket:F2}");
        UI.Separator();

        if (report.Sales.Any())
        {
            Console.WriteLine("  Detalhamento:");
            foreach (var s in report.Sales)
            {
                string type = s.IsOnlineSale ? "Online" : "Presencial";
                Console.WriteLine($"    [{s.Id}] {s.Date:dd/MM/yyyy} | {type} | R$ {s.Total:F2}");
                foreach (var item in s.Items)
                    Console.WriteLine($"         - {item.ProductName} x{item.Quantity} @ R$ {item.UnitPrice:F2} = R$ {item.Subtotal:F2}");
            }
        }
        UI.Pause();
    }

    // ── HELPERS ────────────────────────────────────────────────────────────────
    private void FillCart(ShoppingCart cart, bool isOnline)
    {
        while (true)
        {
            UI.Header("CARRINHO DE COMPRAS");
            var products = _ctx.ProductService.GetAll().ToList();
            foreach (var p in products)
            {
                string unit = p.Unit == SaleUnit.Kilo ? "Kg" : "Un";
                Console.WriteLine($"  [{p.Id}] {p.Name} | R$ {p.Price:F2}/{unit} | Estoque: {p.StockQuantity}");
            }
            Console.WriteLine("\n  [0] Finalizar / Fechar carrinho");
            int pid = UI.ReadInt("ID do produto (0 para finalizar)", 0);
            if (pid == 0) break;

            decimal qty = UI.ReadDecimal("Quantidade");
            try
            {
                _ctx.CartService.AddItem(cart, pid, qty);
                UI.Success("Item adicionado.");
            }
            catch (Exception ex) { UI.Error(ex.Message); }
        }
    }

    private static void ShowCart(ShoppingCart cart)
    {
        UI.Header("RESUMO DO PEDIDO");
        foreach (var item in cart.Items)
            Console.WriteLine($"  {item.Product.Name} x{item.Quantity} = R$ {item.Subtotal:F2}");
        UI.Separator();
        Console.WriteLine($"  TOTAL: R$ {cart.Total:F2}");
        UI.Separator();
    }
}
