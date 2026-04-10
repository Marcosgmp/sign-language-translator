using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Business.Services;

public class EmployeeService
{
    private readonly IEmployeeRepository _repo;
    private readonly IStoreRepository _storeRepo;

    public EmployeeService(IEmployeeRepository repo, IStoreRepository storeRepo)
    {
        _repo = repo;
        _storeRepo = storeRepo;
    }

    public Employee Create(string name, decimal salary, TimeSpan shiftStart, TimeSpan shiftEnd,
                           ContractType contractType, int storeId, IEnumerable<EmployeeRole> roles)
    {
        var store = _storeRepo.GetById(storeId)
            ?? throw new InvalidOperationException($"Store {storeId} not found.");

        var employee = new Employee(name, salary, shiftStart, shiftEnd, contractType, storeId, roles);
        _repo.Add(employee);
        store.AddEmployee(employee);
        return employee;
    }

    public Employee GetById(int id) =>
        _repo.GetById(id) ?? throw new InvalidOperationException($"Employee {id} not found.");

    public IEnumerable<Employee> GetAll() => _repo.GetAll();

    public IEnumerable<Employee> GetByStore(int storeId) => _repo.GetByStore(storeId);

    public IEnumerable<Employee> GetByRole(EmployeeRole role) => _repo.GetByRole(role);

    public void Update(int id, string name, decimal salary, TimeSpan shiftStart, TimeSpan shiftEnd,
                       ContractType contractType, IEnumerable<EmployeeRole> roles)
    {
        var employee = GetById(id);
        employee.Update(name, salary, shiftStart, shiftEnd, contractType, roles);
    }

    public bool Delete(int id)
    {
        var emp = _repo.GetById(id);
        if (emp is null) return false;

        var store = _storeRepo.GetById(emp.StoreId);
        store?.RemoveEmployee(id);
        return _repo.Delete(id);
    }
}
