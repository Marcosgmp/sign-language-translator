using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    T? GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    bool Delete(int id);
}

public interface IEmployeeRepository : IRepository<Employee>
{
    IEnumerable<Employee> GetByStore(int storeId);
    IEnumerable<Employee> GetByRole(EmployeeRole role);
}

public interface IClientRepository : IRepository<Client>
{
    Client? GetByLogin(string login);
    Client? GetByEmail(string email);
}

public interface IProductRepository : IRepository<Product>
{
    IEnumerable<Product> GetByCategory(int categoryId);
    IEnumerable<Product> GetByTag(int tagId);
    IEnumerable<Product> Search(string term);
}

public interface ISaleRepository : IRepository<Sale>
{
    IEnumerable<Sale> GetByStore(int storeId);
    IEnumerable<Sale> GetByClient(int clientId);
    IEnumerable<Sale> GetByYearAndMonth(int year, int month);
    IEnumerable<Sale> GetByYear(int year);
}

public interface ICategoryRepository : IRepository<Category> { }
public interface ITagRepository : IRepository<Tag> { }
public interface IStoreRepository : IRepository<Store> { }
