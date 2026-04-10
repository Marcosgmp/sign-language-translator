namespace Domain.Entities;

public class Company : BaseEntity
{
    public string Name { get; private set; }
    public string Cnpj { get; private set; }
    private readonly List<Store> _stores = new();
    public IReadOnlyList<Store> Stores => _stores.AsReadOnly();

    public Company(string name, string cnpj)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Company name cannot be empty.");
        if (string.IsNullOrWhiteSpace(cnpj)) throw new ArgumentException("CNPJ cannot be empty.");
        Name = name.Trim();
        Cnpj = cnpj.Trim();
    }

    public void AddStore(Store store)
    {
        if (store is null) throw new ArgumentNullException(nameof(store));
        _stores.Add(store);
    }

    public void RemoveStore(int storeId)
    {
        var store = _stores.FirstOrDefault(s => s.Id == storeId);
        if (store is null) throw new InvalidOperationException("Store not found.");
        _stores.Remove(store);
    }

    public void Update(string name, string cnpj)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Company name cannot be empty.");
        if (string.IsNullOrWhiteSpace(cnpj)) throw new ArgumentException("CNPJ cannot be empty.");
        Name = name.Trim();
        Cnpj = cnpj.Trim();
    }
}
