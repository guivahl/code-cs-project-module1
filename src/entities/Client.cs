namespace src;

public sealed class Client
{
    public Account Account { get; init; }
    public string Name { get; set; }
    public string CPF { get; init; }
    public DateTime? DeactivateAt { get; set; }

    public Client(string CPF, string Name, Account account) { 
        this.CPF = CPF;
        this.Name = Name;
        this.Account = account;
    }

    public Client(ClientAccountDto clientAccount) {
        Account account = new Account(clientAccount);

        this.Account = account;
        this.Name = clientAccount.Name;
        this.CPF = clientAccount.CPF;
        this.DeactivateAt = clientAccount.DeactivateAt;
    }

}
