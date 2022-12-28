namespace src;

public sealed class Account
{
    private readonly string DIGITAL_AGENCY_NUMBER = "0001";
    public string AccountNumber { get; init; }
    public string CheckDigit { get; init; }
    public string Agency { get; init; }
    public decimal Balance { get; private set; }

    public Account(string AccountNumber, string CheckDigit)
    {
        this.AccountNumber = AccountNumber;
        this.CheckDigit = CheckDigit;
        this.Agency = this.DIGITAL_AGENCY_NUMBER;

        this.Balance = 0;
    }

    public Account(ClientAccountDto clientAccount) {
        this.AccountNumber = clientAccount.AccountNumber;
        this.CheckDigit = clientAccount.CheckDigit;
        this.Agency = clientAccount.Agency;
        this.Balance = clientAccount.Balance;
    }

    public void AddBalance(decimal balance) =>
        this.Balance += balance;
}
