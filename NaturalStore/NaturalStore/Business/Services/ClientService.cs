using Domain.Entities;
using Domain.Interfaces;

namespace Business.Services;

public class ClientService
{
    private readonly IClientRepository _repo;

    public ClientService(IClientRepository repo) => _repo = repo;

    public Client CreateWalkIn(string name)
    {
        var client = new Client(name);
        _repo.Add(client);
        return client;
    }

    public Client Register(string name, string email, string login, string password)
    {
        if (_repo.GetByLogin(login) != null)
            throw new InvalidOperationException("Login already in use.");
        if (_repo.GetByEmail(email) != null)
            throw new InvalidOperationException("Email already registered.");

        string hash = HashPassword(password);
        var client = new Client(name, email, login, hash);
        _repo.Add(client);
        return client;
    }

    public Client? Login(string login, string password)
    {
        var client = _repo.GetByLogin(login);
        if (client is null) return null;
        return client.CheckPassword(HashPassword(password)) ? client : null;
    }

    public Client GetById(int id) =>
        _repo.GetById(id) ?? throw new InvalidOperationException($"Client {id} not found.");

    public IEnumerable<Client> GetAll() => _repo.GetAll();

    public void Update(int id, string name, string email)
    {
        var client = GetById(id);
        client.Update(name, email);
    }

    public bool Delete(int id) => _repo.Delete(id);

    private static string HashPassword(string password)
    {
        // Simple hash for academic project - in production use BCrypt
        using var sha = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        return Convert.ToHexString(sha.ComputeHash(bytes));
    }
}
