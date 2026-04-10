using Business.Services;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Business;

/// <summary>
/// Wires up all dependencies and acts as the application composition root.
/// In a larger project this would be replaced by a DI container (e.g. Microsoft.Extensions.DependencyInjection).
/// </summary>
public class AppContext
{
    public StoreService StoreService { get; }
    public EmployeeService EmployeeService { get; }
    public ClientService ClientService { get; }
    public CategoryService CategoryService { get; }
    public TagService TagService { get; }
    public ProductService ProductService { get; }
    public CartService CartService { get; }
    public SaleService SaleService { get; }

    public Company Company { get; private set; }

    public AppContext()
    {
        // Repositories
        var storeRepo      = new StoreRepository();
        var employeeRepo   = new EmployeeRepository();
        var clientRepo     = new ClientRepository();
        var categoryRepo   = new CategoryRepository();
        var tagRepo        = new TagRepository();
        var productRepo    = new ProductRepository();
        var saleRepo       = new SaleRepository();

        // Services
        StoreService    = new StoreService(storeRepo);
        EmployeeService = new EmployeeService(employeeRepo, storeRepo);
        ClientService   = new ClientService(clientRepo);
        CategoryService = new CategoryService(categoryRepo);
        TagService      = new TagService(tagRepo);
        ProductService  = new ProductService(productRepo, categoryRepo, tagRepo);
        CartService     = new CartService(productRepo);
        SaleService     = new SaleService(saleRepo, productRepo);

        Company = new Company("Produtos Naturais Ltda.", "00.000.000/0001-00");

        SeedData();
    }

    private void SeedData()
    {
        // Stores
        var aracati = StoreService.Create("Loja Aracati", "Rua das Flores, 100 - Aracati/CE",
            new TimeSpan(8, 0, 0), new TimeSpan(18, 0, 0));
        var russas  = StoreService.Create("Loja Russas",  "Av. Central, 250 - Russas/CE",
            new TimeSpan(8, 0, 0), new TimeSpan(18, 0, 0));

        Company.AddStore(aracati);
        Company.AddStore(russas);

        // Employees - Aracati (2 caixas, 1 gerente)
        EmployeeService.Create("Ana Souza",    2500m, new TimeSpan(8,0,0), new TimeSpan(17,0,0),
            Domain.Enums.ContractType.CLT,   aracati.Id, new[] { Domain.Enums.EmployeeRole.Cashier });
        EmployeeService.Create("Bruno Lima",   2500m, new TimeSpan(9,0,0), new TimeSpan(18,0,0),
            Domain.Enums.ContractType.CLT,   aracati.Id, new[] { Domain.Enums.EmployeeRole.Cashier });
        EmployeeService.Create("Carla Mendes", 4500m, new TimeSpan(8,0,0), new TimeSpan(17,0,0),
            Domain.Enums.ContractType.CLT,   aracati.Id, new[] { Domain.Enums.EmployeeRole.Manager });

        // Employees - Russas (2 caixas, 1 repositor, 1 gerente)
        EmployeeService.Create("Diego Farias", 2500m, new TimeSpan(8,0,0), new TimeSpan(17,0,0),
            Domain.Enums.ContractType.CLT,   russas.Id, new[] { Domain.Enums.EmployeeRole.Cashier });
        EmployeeService.Create("Eva Costa",    2500m, new TimeSpan(9,0,0), new TimeSpan(18,0,0),
            Domain.Enums.ContractType.CLT,   russas.Id, new[] { Domain.Enums.EmployeeRole.Cashier });
        EmployeeService.Create("Felipe Rocha", 2200m, new TimeSpan(8,0,0), new TimeSpan(17,0,0),
            Domain.Enums.ContractType.CLT,   russas.Id, new[] { Domain.Enums.EmployeeRole.Stocker });
        EmployeeService.Create("Gabi Torres",  4500m, new TimeSpan(8,0,0), new TimeSpan(17,0,0),
            Domain.Enums.ContractType.CLT,   russas.Id, new[] { Domain.Enums.EmployeeRole.Manager });

        // Categories & Tags
        var nuts   = CategoryService.Create("Oleaginosas", "Castanhas, amêndoas, etc.");
        var grains = CategoryService.Create("Grãos",       "Sementes e grãos em geral.");
        var honey  = CategoryService.Create("Mel e Derivados", "Produtos apícolas.");

        var tagOrganic  = TagService.Create("Orgânico");
        var tagGlutenFree = TagService.Create("Sem Glúten");
        var tagVegan    = TagService.Create("Vegano");

        // Products
        var cashew = ProductService.Create("Castanha de Caju", 45.00m, Domain.Enums.SaleUnit.Kilo, 50m, nuts.Id);
        var peanut = ProductService.Create("Amendoim",         12.00m, Domain.Enums.SaleUnit.Kilo, 100m, nuts.Id);
        var flax   = ProductService.Create("Linhaça",          18.00m, Domain.Enums.SaleUnit.Kilo, 80m, grains.Id);
        var cashewHoney = ProductService.Create("Mel de Caju",  35.00m, Domain.Enums.SaleUnit.Unit, 30m, honey.Id);

        ProductService.AddTag(cashew.Id, tagOrganic.Id);
        ProductService.AddTag(cashew.Id, tagVegan.Id);
        ProductService.AddTag(peanut.Id, tagGlutenFree.Id);
        ProductService.AddTag(flax.Id,   tagVegan.Id);
        ProductService.AddTag(cashewHoney.Id, tagOrganic.Id);

        // Registered client
        ClientService.Register("João Silva", "joao@email.com", "joao", "123456");
    }
}
