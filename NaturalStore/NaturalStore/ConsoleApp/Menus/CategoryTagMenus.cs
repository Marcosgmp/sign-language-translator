using Business.Services;
using ConsoleApp.Helpers;

namespace ConsoleApp.Menus;

public class CategoryMenu
{
    private readonly CategoryService _service;
    public CategoryMenu(CategoryService service) => _service = service;

    public void Show()
    {
        while (true)
        {
            int opt = UI.Menu("CATEGORIAS", new[] { "Listar", "Cadastrar", "Atualizar", "Remover" });
            switch (opt)
            {
                case 1: List(); break;
                case 2: Create(); break;
                case 3: Update(); break;
                case 4: Delete(); break;
                case 0: return;
            }
        }
    }

    private void List()
    {
        UI.Header("CATEGORIAS");
        foreach (var c in _service.GetAll())
            Console.WriteLine($"  [{c.Id}] {c.Name} - {c.Description}");
        UI.Pause();
    }

    private void Create()
    {
        string name = UI.ReadRequired("Nome");
        string desc = UI.ReadRequired("Descrição");
        try { var c = _service.Create(name, desc); UI.Success($"Categoria '{c.Name}' criada (ID {c.Id})."); }
        catch (Exception ex) { UI.Error(ex.Message); }
        UI.Pause();
    }

    private void Update()
    {
        int id = UI.ReadInt("ID");
        string name = UI.ReadRequired("Nome");
        string desc = UI.ReadRequired("Descrição");
        try { _service.Update(id, name, desc); UI.Success("Atualizada."); }
        catch (Exception ex) { UI.Error(ex.Message); }
        UI.Pause();
    }

    private void Delete()
    {
        int id = UI.ReadInt("ID");
        if (!UI.Confirm("Confirmar?")) return;
        UI.Success(_service.Delete(id) ? "Removida." : "Não encontrada.");
        UI.Pause();
    }
}

public class TagMenu
{
    private readonly TagService _service;
    public TagMenu(TagService service) => _service = service;

    public void Show()
    {
        while (true)
        {
            int opt = UI.Menu("TAGS", new[] { "Listar", "Cadastrar", "Atualizar", "Remover" });
            switch (opt)
            {
                case 1: List(); break;
                case 2: Create(); break;
                case 3: Update(); break;
                case 4: Delete(); break;
                case 0: return;
            }
        }
    }

    private void List()
    {
        UI.Header("TAGS");
        foreach (var t in _service.GetAll())
            Console.WriteLine($"  [{t.Id}] {t.Name}");
        UI.Pause();
    }

    private void Create()
    {
        string name = UI.ReadRequired("Nome");
        try { var t = _service.Create(name); UI.Success($"Tag '{t.Name}' criada (ID {t.Id})."); }
        catch (Exception ex) { UI.Error(ex.Message); }
        UI.Pause();
    }

    private void Update()
    {
        int id = UI.ReadInt("ID");
        string name = UI.ReadRequired("Nome");
        try { _service.Update(id, name); UI.Success("Atualizada."); }
        catch (Exception ex) { UI.Error(ex.Message); }
        UI.Pause();
    }

    private void Delete()
    {
        int id = UI.ReadInt("ID");
        if (!UI.Confirm("Confirmar?")) return;
        UI.Success(_service.Delete(id) ? "Removida." : "Não encontrada.");
        UI.Pause();
    }
}
