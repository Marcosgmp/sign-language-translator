using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Business.Services;

public class ProductService
{
    private readonly IProductRepository _repo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly ITagRepository _tagRepo;

    public ProductService(IProductRepository repo, ICategoryRepository categoryRepo, ITagRepository tagRepo)
    {
        _repo = repo;
        _categoryRepo = categoryRepo;
        _tagRepo = tagRepo;
    }

    public Product Create(string name, decimal price, SaleUnit unit, decimal stock, int categoryId)
    {
        var category = _categoryRepo.GetById(categoryId)
            ?? throw new InvalidOperationException($"Category {categoryId} not found.");
        var product = new Product(name, price, unit, stock, category);
        _repo.Add(product);
        return product;
    }

    public Product GetById(int id) =>
        _repo.GetById(id) ?? throw new InvalidOperationException($"Product {id} not found.");

    public IEnumerable<Product> GetAll() => _repo.GetAll();
    public IEnumerable<Product> GetByCategory(int categoryId) => _repo.GetByCategory(categoryId);
    public IEnumerable<Product> Search(string term) => _repo.Search(term);

    public void Update(int id, string name, decimal price, SaleUnit unit, decimal stock, int categoryId)
    {
        var product = GetById(id);
        var category = _categoryRepo.GetById(categoryId)
            ?? throw new InvalidOperationException($"Category {categoryId} not found.");
        product.Update(name, price, unit, stock, category);
    }

    public void AddTag(int productId, int tagId)
    {
        var product = GetById(productId);
        var tag = _tagRepo.GetById(tagId) ?? throw new InvalidOperationException($"Tag {tagId} not found.");
        product.AddTag(tag);
    }

    public void RemoveTag(int productId, int tagId) => GetById(productId).RemoveTag(tagId);

    public bool Delete(int id) => _repo.Delete(id);
}
