namespace src;

public sealed class Account
{
    public static readonly string DIGITAL_AGENCY_NUMBER = "0001";
    public static readonly string BANK_CODE = "777";
    public string AccountNumber { get; init; }
    public string BankCode { get; init; }
    public string CheckDigit { get; init; }
    public string AccoutNumberWithDigit { get => $"{AccountNumber}{CheckDigit}"; }
    public string Agency { get; init; }
    public decimal Balance { get; private set; }

    public Account(string AccountNumber, string CheckDigit, decimal Balance = 0)
    {
        this.AccountNumber = AccountNumber;
        this.CheckDigit = CheckDigit;
        this.Agency = Account.DIGITAL_AGENCY_NUMBER;
        this.BankCode = Account.BANK_CODE;

        this.Balance = Balance;
    }

    public Account(ClientAccountDto clientAccount)
    {
        this.AccountNumber = clientAccount.AccountNumber;
        this.CheckDigit = clientAccount.CheckDigit;
        this.Agency = clientAccount.Agency;
        this.Balance = clientAccount.Balance;
        this.BankCode = Account.BANK_CODE;
    }

    public void AddBalance(decimal balance) =>
        this.Balance += balance;

    public void DecrementBalance(decimal balance) =>
        this.Balance -= balance;
}
