using Business.Services;
using ConsoleApp.Helpers;
using Domain.Enums;

namespace ConsoleApp.Menus;

public class ProductMenu
{
    private readonly ProductService _productService;
    private readonly CategoryService _categoryService;
    private readonly TagService _tagService;

    public ProductMenu(ProductService ps, CategoryService cs, TagService ts)
    {
        _productService  = ps;
        _categoryService = cs;
        _tagService      = ts;
    }

    public void Show()
    {
        while (true)
        {
            int opt = UI.Menu("GERENCIAMENTO DE PRODUTOS",
                new[] { "Listar todos", "Buscar por nome", "Cadastrar", "Atualizar", "Remover",
                        "Adicionar tag", "Remover tag" });
            switch (opt)
            {
                case 1: List(); break;
                case 2: Search(); break;
                case 3: Create(); break;
                case 4: Update(); break;
                case 5: Delete(); break;
                case 6: AddTag(); break;
                case 7: RemoveTag(); break;
                case 0: return;
            }
        }
    }

    private void List()
    {
        UI.Header("PRODUTOS");
        var products = _productService.GetAll().ToList();
        if (!products.Any()) { UI.Info("Nenhum produto cadastrado."); UI.Pause(); return; }
        foreach (var p in products)
        {
            string unit = p.Unit == SaleUnit.Kilo ? "Kg" : "Un";
            string tags = p.Tags.Any() ? string.Join(", ", p.Tags.Select(t => t.Name)) : "-";
            Console.WriteLine($"  [{p.Id}] {p.Name} | R$ {p.Price:F2}/{unit} | Estoque: {p.StockQuantity} | Cat: {p.Category.Name} | Tags: {tags}");
        }
        UI.Pause();
    }

    private void Search()
    {
        string term = UI.ReadRequired("Termo de busca");
        var products = _productService.Search(term).ToList();
        if (!products.Any()) { UI.Info("Nenhum produto encontrado."); UI.Pause(); return; }
        foreach (var p in products)
            Console.WriteLine($"  [{p.Id}] {p.Name} | R$ {p.Price:F2} | Estoque: {p.StockQuantity}");
        UI.Pause();
    }

    private void Create()
    {
        UI.Header("CADASTRAR PRODUTO");
        var categories = _categoryService.GetAll().ToList();
        if (!categories.Any()) { UI.Error("Cadastre uma categoria primeiro."); UI.Pause(); return; }
        foreach (var c in categories) Console.WriteLine($"  [{c.Id}] {c.Name}");

        string name   = UI.ReadRequired("Nome");
        decimal price = UI.ReadDecimal("Preço", 0);
        decimal stock = UI.ReadDecimal("Estoque inicial", 0);
        Console.WriteLine("  Unidade: [1] Unidade  [2] Quilo");
        var unit = UI.ReadInt("Opção", 1, 2) == 1 ? SaleUnit.Unit : SaleUnit.Kilo;
        int catId = UI.ReadInt("ID da categoria");

        try
        {
            var p = _productService.Create(name, price, unit, stock, catId);
            UI.Success($"Produto '{p.Name}' cadastrado com ID {p.Id}.");
        }
        catch (Exception ex) { UI.Error(ex.Message); }
        UI.Pause();
    }

    private void Update()
    {
        int id = UI.ReadInt("ID do produto");
        try
        {
            var p = _productService.GetById(id);
            string name   = UI.ReadRequired($"Nome [{p.Name}]");
            decimal price = UI.ReadDecimal("Preço", 0);
            decimal stock = UI.ReadDecimal("Estoque", 0);
            Console.WriteLine("  Unidade: [1] Unidade  [2] Quilo");
            var unit = UI.ReadInt("Opção", 1, 2) == 1 ? SaleUnit.Unit : SaleUnit.Kilo;
            int catId = UI.ReadInt("ID da categoria");
            _productService.Update(id, name, price, unit, stock, catId);
            UI.Success("Produto atualizado.");
        }
        catch (Exception ex) { UI.Error(ex.Message); }
        UI.Pause();
    }

    private void Delete()
    {
        int id = UI.ReadInt("ID do produto");
        if (!UI.Confirm("Confirmar remoção?")) return;
        UI.Success(_productService.Delete(id) ? "Removido." : "Produto não encontrado.");
        UI.Pause();
    }

    private void AddTag()
    {
        int productId = UI.ReadInt("ID do produto");
        var tags = _tagService.GetAll().ToList();
        if (!tags.Any()) { UI.Info("Nenhuma tag cadastrada."); UI.Pause(); return; }
        foreach (var t in tags) Console.WriteLine($"  [{t.Id}] {t.Name}");
        int tagId = UI.ReadInt("ID da tag");
        try { _productService.AddTag(productId, tagId); UI.Success("Tag adicionada."); }
        catch (Exception ex) { UI.Error(ex.Message); }
        UI.Pause();
    }

    private void RemoveTag()
    {
        int productId = UI.ReadInt("ID do produto");
        int tagId = UI.ReadInt("ID da tag");
        try { _productService.RemoveTag(productId, tagId); UI.Success("Tag removida."); }
        catch (Exception ex) { UI.Error(ex.Message); }
        UI.Pause();
    }
}
