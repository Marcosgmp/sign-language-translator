using Domain.Entities;
using Domain.Interfaces;

namespace Business.Services;

public class StoreService
{
    private readonly IStoreRepository _repo;
    public StoreService(IStoreRepository repo) => _repo = repo;

    public Store Create(string name, string address, TimeSpan openingTime, TimeSpan closingTime)
    {
        var store = new Store(name, address, openingTime, closingTime);
        _repo.Add(store);
        return store;
    }

    public Store GetById(int id) =>
        _repo.GetById(id) ?? throw new InvalidOperationException($"Store {id} not found.");

    public IEnumerable<Store> GetAll() => _repo.GetAll();

    public void Update(int id, string name, string address, TimeSpan openingTime, TimeSpan closingTime)
    {
        var store = GetById(id);
        store.Update(name, address, openingTime, closingTime);
    }

    public bool Delete(int id) => _repo.Delete(id);
}
