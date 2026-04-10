using Business.Services;
using ConsoleApp.Helpers;
using Domain.Enums;

namespace ConsoleApp.Menus;

public class EmployeeMenu
{
    private readonly EmployeeService _service;
    private readonly StoreService _storeService;

    public EmployeeMenu(EmployeeService service, StoreService storeService)
    {
        _service = service;
        _storeService = storeService;
    }

    public void Show()
    {
        while (true)
        {
            int opt = UI.Menu("GERENCIAMENTO DE FUNCIONÁRIOS",
                new[] { "Listar todos", "Buscar por ID", "Cadastrar", "Atualizar", "Remover" });
            switch (opt)
            {
                case 1: List(); break;
                case 2: GetById(); break;
                case 3: Create(); break;
                case 4: Update(); break;
                case 5: Delete(); break;
                case 0: return;
            }
        }
    }

    private void List()
    {
        UI.Header("FUNCIONÁRIOS");
        var employees = _service.GetAll().ToList();
        if (!employees.Any()) { UI.Info("Nenhum funcionário cadastrado."); UI.Pause(); return; }
        foreach (var e in employees)
        {
            var roles = string.Join(", ", e.Roles.Select(r => r switch
            {
                EmployeeRole.Cashier   => "Caixa",
                EmployeeRole.Stocker   => "Repositor",
                EmployeeRole.Manager   => "Gerente",
                EmployeeRole.Deliverer => "Entregador",
                _ => r.ToString()
            }));
            Console.WriteLine($"  [{e.Id}] {e.Name} | {roles} | R$ {e.Salary:F2} | {e.ContractType} | Loja {e.StoreId}");
        }
        UI.Pause();
    }

    private void GetById()
    {
        int id = UI.ReadInt("ID do funcionário");
        try
        {
            var e = _service.GetById(id);
            var roles = string.Join(", ", e.Roles);
            UI.Header($"Funcionário #{e.Id}");
            Console.WriteLine($"  Nome:       {e.Name}");
            Console.WriteLine($"  Funções:    {roles}");
            Console.WriteLine($"  Salário:    R$ {e.Salary:F2}");
            Console.WriteLine($"  Horário:    {e.ShiftStart:hh\\:mm} - {e.ShiftEnd:hh\\:mm}");
            Console.WriteLine($"  Contrato:   {e.ContractType}");
            Console.WriteLine($"  Loja ID:    {e.StoreId}");
        }
        catch (Exception ex) { UI.Error(ex.Message); }
        UI.Pause();
    }

    private void Create()
    {
        UI.Header("CADASTRAR FUNCIONÁRIO");
        var stores = _storeService.GetAll().ToList();
        if (!stores.Any()) { UI.Error("Nenhuma loja cadastrada."); UI.Pause(); return; }

        foreach (var s in stores) Console.WriteLine($"  [{s.Id}] {s.Name}");
        int storeId = UI.ReadInt("Loja (ID)");

        string name = UI.ReadRequired("Nome");
        decimal salary = UI.ReadDecimal("Salário", 0);
        TimeSpan start = UI.ReadTime("Hora de entrada");
        TimeSpan end   = UI.ReadTime("Hora de saída");
        var contract   = ReadContractType();
        var roles      = ReadRoles();

        try
        {
            var e = _service.Create(name, salary, start, end, contract, storeId, roles);
            UI.Success($"Funcionário '{e.Name}' cadastrado com ID {e.Id}.");
        }
        catch (Exception ex) { UI.Error(ex.Message); }
        UI.Pause();
    }

    private void Update()
    {
        int id = UI.ReadInt("ID do funcionário a atualizar");
        try
        {
            var e = _service.GetById(id);
            UI.Header($"Atualizar: {e.Name}");
            string name    = UI.ReadRequired("Novo nome");
            decimal salary = UI.ReadDecimal("Novo salário", 0);
            TimeSpan start = UI.ReadTime("Nova hora de entrada");
            TimeSpan end   = UI.ReadTime("Nova hora de saída");
            var contract   = ReadContractType();
            var roles      = ReadRoles();
            _service.Update(id, name, salary, start, end, contract, roles);
            UI.Success("Funcionário atualizado.");
        }
        catch (Exception ex) { UI.Error(ex.Message); }
        UI.Pause();
    }

    private void Delete()
    {
        int id = UI.ReadInt("ID do funcionário a remover");
        if (!UI.Confirm("Confirmar remoção?")) return;
        UI.Success(_service.Delete(id) ? "Removido com sucesso." : "Funcionário não encontrado.");
        UI.Pause();
    }

    private static ContractType ReadContractType()
    {
        Console.WriteLine("  Regime: [1] CLT  [2] CNPJ");
        return UI.ReadInt("Opção", 1, 2) == 1 ? ContractType.CLT : ContractType.CNPJ;
    }

    private static List<EmployeeRole> ReadRoles()
    {
        var roles = new List<EmployeeRole>();
        Console.WriteLine("  Funções disponíveis: [1] Caixa  [2] Repositor  [3] Gerente  [4] Entregador");
        Console.WriteLine("  Digite os números separados por vírgula (ex: 1,3):");
        Console.Write("  Funções: ");
        var input = Console.ReadLine() ?? "";
        foreach (var part in input.Split(','))
        {
            if (int.TryParse(part.Trim(), out int n))
            {
                var role = n switch { 1 => EmployeeRole.Cashier, 2 => EmployeeRole.Stocker,
                    3 => EmployeeRole.Manager, 4 => EmployeeRole.Deliverer, _ => (EmployeeRole?)null };
                if (role.HasValue && !roles.Contains(role.Value)) roles.Add(role.Value);
            }
        }
        if (!roles.Any()) roles.Add(EmployeeRole.Cashier);
        return roles;
    }
}
