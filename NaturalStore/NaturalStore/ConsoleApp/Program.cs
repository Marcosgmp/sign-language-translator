using ConsoleApp.Helpers;
using ConsoleApp.Menus;
using BizContext = Business.AppContext;

var ctx = new BizContext();

var storeMenu    = new StoreMenu(ctx.StoreService);
var employeeMenu = new EmployeeMenu(ctx.EmployeeService, ctx.StoreService);
var clientMenu   = new ClientMenu(ctx.ClientService);
var categoryMenu = new CategoryMenu(ctx.CategoryService);
var tagMenu      = new TagMenu(ctx.TagService);
var productMenu  = new ProductMenu(ctx.ProductService, ctx.CategoryService, ctx.TagService);
var saleMenu     = new SaleMenu(ctx);

Console.OutputEncoding = System.Text.Encoding.UTF8;

while (true)
{
    int opt = UI.Menu("SISTEMA DE PRODUTOS NATURAIS",
        new[]
        {
            "Lojas",
            "Funcionários",
            "Clientes",
            "Categorias",
            "Tags",
            "Produtos",
            "Vendas / PDV"
        });

    switch (opt)
    {
        case 1: storeMenu.Show(); break;
        case 2: employeeMenu.Show(); break;
        case 3: clientMenu.Show(); break;
        case 4: categoryMenu.Show(); break;
        case 5: tagMenu.Show(); break;
        case 6: productMenu.Show(); break;
        case 7: saleMenu.Show(); break;
        case 0:
            UI.Info("Encerrando sistema. Até logo!");
            return;
    }
}
