namespace src;

public sealed class Account
{
    private readonly string DIGITAL_AGENCY_NUMBER = "0001";
    public string AccountNumber { get; init; }
    public string CheckDigit { get; init; }
    public string Agency { get; init; }
    public decimal Balance { get; private set; }

    public Account(string accountNumber, string checkDigit)
    {
        this.AccountNumber = accountNumber;
        this.CheckDigit = checkDigit;
        this.Agency = this.DIGITAL_AGENCY_NUMBER;

        this.Balance = 0;
    }

    public void AddBalance(decimal balance) =>
        this.Balance += balance;
}
