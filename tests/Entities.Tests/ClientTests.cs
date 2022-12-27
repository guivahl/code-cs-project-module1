namespace Entities.Tests
{
    public class ClientTests
    {
        [Fact]
        public void Client_CreateNewClient()
        {
            string accountNumber = Utils.RandomStringOfNumbers(5);
            string checkDigit = Utils.RandomStringOfNumbers(1);

            Account account = new Account(accountNumber, checkDigit);

            string CPF = Utils.RandomStringOfNumbers(11);
            string name = Utils.RandomString(20);

            Client client = new Client(CPF, name, account);

            bool isBalanceZero = client.Account.Balance == 0;
            bool isCpfCorrectLth = client.CPF.Length == 11;

            Assert.NotNull(client.Account);
            Assert.True(isBalanceZero);
            Assert.True(isCpfCorrectLth);
        }
    }
}