namespace Entities.Tests
{
    public class AccountTests
    {
        [Fact]
        public void Accounts_CreateNewAccount()
        {
            string accountNumber = Utils.RandomStringOfNumbers(5);
            string checkDigit = Utils.RandomStringOfNumbers(1);

            Account account = new Account(accountNumber, checkDigit);

            bool isAccountNumberLengthCorret = account.AccountNumber.Length == 5;
            bool isCheckDigitLengthCorret = account.CheckDigit.Length == 1;
            bool isBalanceZero = account.Balance == 0;

            Assert.True(isAccountNumberLengthCorret);
            Assert.True(isCheckDigitLengthCorret);
            Assert.True(isBalanceZero);
        }
        [Fact]
        public void Accounts_AddBalance()
        {
            string accountNumber = Utils.RandomStringOfNumbers(5);
            string checkDigit = Utils.RandomStringOfNumbers(1);

            Account account = new Account(accountNumber, checkDigit);

            decimal v1 = 50, v2 = 30, v3 = 20;
            
            account.AddBalance(v1);
            account.AddBalance(v2);
            account.AddBalance(v3);

            Assert.Equal<decimal>(account.Balance, v1 + v2 + v3);
        }
    }
}