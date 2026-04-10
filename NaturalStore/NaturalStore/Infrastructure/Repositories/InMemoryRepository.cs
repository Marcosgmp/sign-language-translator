using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories;

public abstract class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly List<T> _data = new();

    public T? GetById(int id) => _data.FirstOrDefault(e => e.Id == id);
    public IEnumerable<T> GetAll() => _data.AsReadOnly();

    public void Add(T entity)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));
        _data.Add(entity);
    }

    public virtual void Update(T entity)
    {
        // Entity is a reference type; mutation is done via domain methods.
        // This method exists to satisfy the interface and can persist to storage if needed.
        if (!_data.Any(e => e.Id == entity.Id))
            throw new InvalidOperationException("Entity not found.");
    }

    public bool Delete(int id)
    {
        var entity = _data.FirstOrDefault(e => e.Id == id);
        if (entity is null) return false;
        _data.Remove(entity);
        return true;
    }
}
