using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Infrastructure.Repositories;

public class EmployeeRepository : InMemoryRepository<Employee>, IEmployeeRepository
{
    public IEnumerable<Employee> GetByStore(int storeId) =>
        _data.Where(e => e.StoreId == storeId);

    public IEnumerable<Employee> GetByRole(EmployeeRole role) =>
        _data.Where(e => e.HasRole(role));
}

public class ClientRepository : InMemoryRepository<Client>, IClientRepository
{
    public Client? GetByLogin(string login) =>
        _data.FirstOrDefault(c => c.Login == login);

    public Client? GetByEmail(string email) =>
        _data.FirstOrDefault(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
}

public class ProductRepository : InMemoryRepository<Product>, IProductRepository
{
    public IEnumerable<Product> GetByCategory(int categoryId) =>
        _data.Where(p => p.Category.Id == categoryId);

    public IEnumerable<Product> GetByTag(int tagId) =>
        _data.Where(p => p.Tags.Any(t => t.Id == tagId));

    public IEnumerable<Product> Search(string term) =>
        _data.Where(p => p.Name.Contains(term, StringComparison.OrdinalIgnoreCase));
}

public class SaleRepository : InMemoryRepository<Sale>, ISaleRepository
{
    public IEnumerable<Sale> GetByStore(int storeId) =>
        _data.Where(s => s.StoreId == storeId);

    public IEnumerable<Sale> GetByClient(int clientId) =>
        _data.Where(s => s.Client?.Id == clientId);

    public IEnumerable<Sale> GetByYearAndMonth(int year, int month) =>
        _data.Where(s => s.Date.Year == year && s.Date.Month == month);

    public IEnumerable<Sale> GetByYear(int year) =>
        _data.Where(s => s.Date.Year == year);
}

public class CategoryRepository : InMemoryRepository<Category>, ICategoryRepository { }

public class TagRepository : InMemoryRepository<Tag>, ITagRepository { }

public class StoreRepository : InMemoryRepository<Store>, IStoreRepository { }
