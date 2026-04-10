using Domain.Entities;
using Domain.Interfaces;

namespace Business.Services;

public class CategoryService
{
    private readonly ICategoryRepository _repo;
    public CategoryService(ICategoryRepository repo) => _repo = repo;

    public Category Create(string name, string description = "")
    {
        var category = new Category(name, description);
        _repo.Add(category);
        return category;
    }

    public Category GetById(int id) =>
        _repo.GetById(id) ?? throw new InvalidOperationException($"Category {id} not found.");

    public IEnumerable<Category> GetAll() => _repo.GetAll();

    public void Update(int id, string name, string description)
    {
        var cat = GetById(id);
        cat.Update(name, description);
    }

    public bool Delete(int id) => _repo.Delete(id);
}
