namespace Domain.Entities;

public class Store : BaseEntity
{
    public string Name { get; private set; }
    public string Address { get; private set; }
    public TimeSpan OpeningTime { get; private set; }
    public TimeSpan ClosingTime { get; private set; }

    private readonly List<Employee> _employees = new();
    public IReadOnlyList<Employee> Employees => _employees.AsReadOnly();

    public Store(string name, string address, TimeSpan openingTime, TimeSpan closingTime)
    {
        Validate(name, address);
        Name = name.Trim();
        Address = address.Trim();
        OpeningTime = openingTime;
        ClosingTime = closingTime;
    }

    public void Update(string name, string address, TimeSpan openingTime, TimeSpan closingTime)
    {
        Validate(name, address);
        Name = name.Trim();
        Address = address.Trim();
        OpeningTime = openingTime;
        ClosingTime = closingTime;
    }

    public void AddEmployee(Employee employee)
    {
        if (employee is null) throw new ArgumentNullException(nameof(employee));
        if (_employees.Any(e => e.Id == employee.Id))
            throw new InvalidOperationException("Employee already assigned to this store.");
        _employees.Add(employee);
    }

    public void RemoveEmployee(int employeeId)
    {
        var emp = _employees.FirstOrDefault(e => e.Id == employeeId);
        if (emp is null) throw new InvalidOperationException("Employee not found in this store.");
        _employees.Remove(emp);
    }

    private static void Validate(string name, string address)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Store name cannot be empty.");
        if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Address cannot be empty.");
    }
}
