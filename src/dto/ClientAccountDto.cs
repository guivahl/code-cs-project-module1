namespace src;

public class ClientAccountDto
{
    public string Name { get; init; }
    public string CPF { get; init; }
    public DateTime? DeactivateAt { get; init; }
    public string AccountNumber { get; init; }
    public string CheckDigit { get; init; }
    public string Agency { get; init; }
    public decimal Balance { get; init; }


    public ClientAccountDto(Client client, Account account)
    {
        this.AccountNumber = account.AccountNumber;
        this.CheckDigit = account.CheckDigit;
        this.Agency = account.Agency;
        this.Balance = account.Balance;

        this.Name = client.Name;
        this.CPF = client.CPF;
        this.DeactivateAt = client.DeactivateAt;
    }

}