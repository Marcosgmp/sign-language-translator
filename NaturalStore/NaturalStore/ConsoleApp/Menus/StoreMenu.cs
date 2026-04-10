using Business.Services;
using ConsoleApp.Helpers;

namespace ConsoleApp.Menus;

public class StoreMenu
{
    private readonly StoreService _service;
    public StoreMenu(StoreService service) => _service = service;

    public void Show()
    {
        while (true)
        {
            int opt = UI.Menu("LOJAS", new[] { "Listar", "Detalhes", "Cadastrar", "Atualizar", "Remover" });
            switch (opt)
            {
                case 1: List(); break;
                case 2: Details(); break;
                case 3: Create(); break;
                case 4: Update(); break;
                case 5: Delete(); break;
                case 0: return;
            }
        }
    }

    private void List()
    {
        UI.Header("LOJAS");
        foreach (var s in _service.GetAll())
            Console.WriteLine($"  [{s.Id}] {s.Name} | {s.Address} | {s.OpeningTime:hh\\:mm}-{s.ClosingTime:hh\\:mm} | {s.Employees.Count} funcionário(s)");
        UI.Pause();
    }

    private void Details()
    {
        int id = UI.ReadInt("ID da loja");
        try
        {
            var s = _service.GetById(id);
            UI.Header(s.Name);
            Console.WriteLine($"  Endereço:      {s.Address}");
            Console.WriteLine($"  Funcionamento: {s.OpeningTime:hh\\:mm} às {s.ClosingTime:hh\\:mm}");
            Console.WriteLine($"  Funcionários ({s.Employees.Count}):");
            foreach (var e in s.Employees)
                Console.WriteLine($"    - [{e.Id}] {e.Name} ({string.Join(", ", e.Roles)})");
        }
        catch (Exception ex) { UI.Error(ex.Message); }
        UI.Pause();
    }

    private void Create()
    {
        string name    = UI.ReadRequired("Nome");
        string address = UI.ReadRequired("Endereço");
        TimeSpan open  = UI.ReadTime("Abertura");
        TimeSpan close = UI.ReadTime("Fechamento");
        try { var s = _service.Create(name, address, open, close); UI.Success($"Loja criada (ID {s.Id})."); }
        catch (Exception ex) { UI.Error(ex.Message); }
        UI.Pause();
    }

    private void Update()
    {
        int id         = UI.ReadInt("ID");
        string name    = UI.ReadRequired("Nome");
        string address = UI.ReadRequired("Endereço");
        TimeSpan open  = UI.ReadTime("Abertura");
        TimeSpan close = UI.ReadTime("Fechamento");
        try { _service.Update(id, name, address, open, close); UI.Success("Loja atualizada."); }
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
