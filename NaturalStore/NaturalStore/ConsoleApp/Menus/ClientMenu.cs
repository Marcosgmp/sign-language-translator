using Business.Services;
using ConsoleApp.Helpers;

namespace ConsoleApp.Menus;

public class ClientMenu
{
    private readonly ClientService _service;

    public ClientMenu(ClientService service) => _service = service;

    public void Show()
    {
        while (true)
        {
            int opt = UI.Menu("GERENCIAMENTO DE CLIENTES",
                new[] { "Listar todos", "Buscar por ID", "Cadastrar cliente online", "Atualizar", "Remover" });
            switch (opt)
            {
                case 1: List(); break;
                case 2: GetById(); break;
                case 3: Register(); break;
                case 4: Update(); break;
                case 5: Delete(); break;
                case 0: return;
            }
        }
    }

    private void List()
    {
        UI.Header("CLIENTES");
        var clients = _service.GetAll().ToList();
        if (!clients.Any()) { UI.Info("Nenhum cliente cadastrado."); UI.Pause(); return; }
        foreach (var c in clients)
        {
            string type = c.IsOnline ? "Online" : "Presencial";
            Console.WriteLine($"  [{c.Id}] {c.Name} | {c.Email} | {type}");
        }
        UI.Pause();
    }

    private void GetById()
    {
        int id = UI.ReadInt("ID do cliente");
        try
        {
            var c = _service.GetById(id);
            UI.Header($"Cliente #{c.Id}");
            Console.WriteLine($"  Nome:   {c.Name}");
            Console.WriteLine($"  Email:  {c.Email}");
            Console.WriteLine($"  Login:  {c.Login ?? "(sem login)"}");
            Console.WriteLine($"  Compras: {c.PurchaseHistory.Count}");
        }
        catch (Exception ex) { UI.Error(ex.Message); }
        UI.Pause();
    }

    private void Register()
    {
        UI.Header("CADASTRAR CLIENTE");
        string name  = UI.ReadRequired("Nome");
        string email = UI.ReadRequired("Email");
        string login = UI.ReadRequired("Login");
        string pass  = UI.ReadRequired("Senha");
        try
        {
            var c = _service.Register(name, email, login, pass);
            UI.Success($"Cliente '{c.Name}' cadastrado com ID {c.Id}.");
        }
        catch (Exception ex) { UI.Error(ex.Message); }
        UI.Pause();
    }

    private void Update()
    {
        int id = UI.ReadInt("ID do cliente");
        try
        {
            var c = _service.GetById(id);
            string name  = UI.ReadRequired($"Nome [{c.Name}]");
            string email = UI.ReadRequired($"Email [{c.Email}]");
            _service.Update(id, name, email);
            UI.Success("Cliente atualizado.");
        }
        catch (Exception ex) { UI.Error(ex.Message); }
        UI.Pause();
    }

    private void Delete()
    {
        int id = UI.ReadInt("ID do cliente");
        if (!UI.Confirm("Confirmar remoção?")) return;
        UI.Success(_service.Delete(id) ? "Removido." : "Cliente não encontrado.");
        UI.Pause();
    }
}
