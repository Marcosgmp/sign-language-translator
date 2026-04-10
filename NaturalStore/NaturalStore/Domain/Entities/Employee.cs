using Domain.Enums;

namespace Domain.Entities;

public class Employee : BaseEntity
{
    public string Name { get; private set; }
    public decimal Salary { get; private set; }
    public TimeSpan ShiftStart { get; private set; }
    public TimeSpan ShiftEnd { get; private set; }
    public ContractType ContractType { get; private set; }
    public int StoreId { get; private set; }

    private readonly List<EmployeeRole> _roles = new();
    public IReadOnlyList<EmployeeRole> Roles => _roles.AsReadOnly();

    public Employee(string name, decimal salary, TimeSpan shiftStart, TimeSpan shiftEnd,
                    ContractType contractType, int storeId, IEnumerable<EmployeeRole> roles)
    {
        Validate(name, salary, roles);
        Name = name.Trim();
        Salary = salary;
        ShiftStart = shiftStart;
        ShiftEnd = shiftEnd;
        ContractType = contractType;
        StoreId = storeId;
        _roles.AddRange(roles.Distinct());
    }

    private static void Validate(string name, decimal salary, IEnumerable<EmployeeRole> roles)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Employee name cannot be empty.");
        if (salary < 0) throw new ArgumentException("Salary cannot be negative.");
        if (roles == null || !roles.Any()) throw new ArgumentException("Employee must have at least one role.");
    }

    public void Update(string name, decimal salary, TimeSpan shiftStart, TimeSpan shiftEnd,
                       ContractType contractType, IEnumerable<EmployeeRole> roles)
    {
        Validate(name, salary, roles);
        Name = name.Trim();
        Salary = salary;
        ShiftStart = shiftStart;
        ShiftEnd = shiftEnd;
        ContractType = contractType;
        _roles.Clear();
        _roles.AddRange(roles.Distinct());
    }

    public bool HasRole(EmployeeRole role) => _roles.Contains(role);

    public void AddRole(EmployeeRole role)
    {
        if (!_roles.Contains(role)) _roles.Add(role);
    }

    public void RemoveRole(EmployeeRole role)
    {
        if (_roles.Count == 1) throw new InvalidOperationException("Employee must have at least one role.");
        _roles.Remove(role);
    }
}
