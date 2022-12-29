namespace src;

public sealed class Client
{
    public Account Account { get; init; }
    public string Name { get; set; }
    public string CPF { get; init; }
    public string? DeactivateAt { get; set; }

    public Client(string CPF, string Name, Account account) { 
        this.CPF = CPF;
        this.Name = Name;
        this.Account = account;
    }

    public Client(ClientAccountDto clientAccount) {
        Account account = new Account(clientAccount);
        System.Console.WriteLine($"AccountNumber: {clientAccount.AccountNumber}");
        System.Console.WriteLine($"CheckDigit: {clientAccount.CheckDigit}");
        System.Console.WriteLine($"Agency: {clientAccount.Agency}");
        System.Console.WriteLine($"Name: {clientAccount.Name}");
        System.Console.WriteLine($"CPF: {clientAccount.CPF}");
        System.Console.WriteLine($"DeactivateAt: {clientAccount.DeactivateAt}");
        this.Account = account;
        this.Name = clientAccount.Name;
        this.CPF = clientAccount.CPF;
        this.DeactivateAt = clientAccount.DeactivateAt;
    }

}
