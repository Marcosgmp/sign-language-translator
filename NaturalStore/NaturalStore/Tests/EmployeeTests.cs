using Domain.Entities;
using Domain.Enums;
using Xunit;

namespace Tests;

public class EmployeeTests
{
    private static Employee MakeEmployee(IEnumerable<EmployeeRole>? roles = null) =>
        new Employee("João", 2500m, new TimeSpan(8, 0, 0), new TimeSpan(17, 0, 0),
            ContractType.CLT, 1, roles ?? new[] { EmployeeRole.Cashier });

    [Fact]
    public void CreateEmployee_ValidData_ShouldSucceed()
    {
        var e = MakeEmployee();
        Assert.Equal("João", e.Name);
        Assert.Contains(EmployeeRole.Cashier, e.Roles);
    }

    [Fact]
    public void CreateEmployee_EmptyName_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() =>
            new Employee("", 2500m, TimeSpan.Zero, TimeSpan.Zero, ContractType.CLT, 1,
                new[] { EmployeeRole.Cashier }));
    }

    [Fact]
    public void CreateEmployee_NoRoles_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() =>
            new Employee("João", 2500m, TimeSpan.Zero, TimeSpan.Zero, ContractType.CLT, 1,
                Array.Empty<EmployeeRole>()));
    }

    [Fact]
    public void AddRole_ShouldAllowMultipleRoles()
    {
        var e = MakeEmployee();
        e.AddRole(EmployeeRole.Manager);
        Assert.True(e.HasRole(EmployeeRole.Cashier));
        Assert.True(e.HasRole(EmployeeRole.Manager));
    }

    [Fact]
    public void RemoveLastRole_ShouldThrow()
    {
        var e = MakeEmployee(new[] { EmployeeRole.Cashier });
        Assert.Throws<InvalidOperationException>(() => e.RemoveRole(EmployeeRole.Cashier));
    }

    [Fact]
    public void Update_ShouldChangeData()
    {
        var e = MakeEmployee();
        e.Update("Maria", 3000m, TimeSpan.Zero, TimeSpan.Zero, ContractType.CNPJ,
            new[] { EmployeeRole.Manager });
        Assert.Equal("Maria", e.Name);
        Assert.Contains(EmployeeRole.Manager, e.Roles);
    }
}
