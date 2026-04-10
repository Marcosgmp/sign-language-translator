namespace Domain.Entities;

public class Client : BaseEntity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string? Login { get; private set; }
    private string? _passwordHash;
    public bool IsOnline => Login != null;

    private readonly List<Sale> _purchaseHistory = new();
    public IReadOnlyList<Sale> PurchaseHistory => _purchaseHistory.AsReadOnly();

    // Walk-in client (no account)
    public Client(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Client name cannot be empty.");
        Name = name.Trim();
        Email = string.Empty;
    }

    // Registered client
    public Client(string name, string email, string login, string passwordHash)
    {
        ValidateRegistered(name, email, login, passwordHash);
        Name = name.Trim();
        Email = email.Trim();
        Login = login.Trim();
        _passwordHash = passwordHash;
    }

    public void Update(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be empty.");
        Name = name.Trim();
        Email = email.Trim();
    }

    public void SetCredentials(string login, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(login)) throw new ArgumentException("Login cannot be empty.");
        if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Password cannot be empty.");
        Login = login.Trim();
        _passwordHash = passwordHash;
    }

    public bool CheckPassword(string passwordHash) => _passwordHash == passwordHash;

    public void AddPurchase(Sale sale)
    {
        if (sale is null) throw new ArgumentNullException(nameof(sale));
        _purchaseHistory.Add(sale);
    }

    private static void ValidateRegistered(string name, string email, string login, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be empty.");
        if (string.IsNullOrWhiteSpace(login)) throw new ArgumentException("Login cannot be empty.");
        if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Password hash cannot be empty.");
    }
}
