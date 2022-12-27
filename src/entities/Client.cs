namespace src;

public class Client
{
    public Account Account { get; init; }
    public string Name { get; set; }
    public string CPF { get; init; }
    public DateTime? DeactivateAt { get; set; }

    public Client(string CPF, string name, Account account) { 
        this.CPF = CPF;
        this.Name = name;
        this.Account = account;
    }
}
