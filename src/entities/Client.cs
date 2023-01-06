namespace src;

public sealed class Client
{
    public Account Account { get; init; }
    public string Name { get; set; }
    public string CPF { get; init; }
    public DateTime? DeactivateAt { get; set; }

    public Client(string CPF, string Name, Account account, DateTime? DeactivateAt = null)
    {
        this.CPF = CPF;
        this.Name = Name;
        this.Account = account;
        this.DeactivateAt = DeactivateAt;
    }
}
