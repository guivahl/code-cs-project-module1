namespace Repositories.Tests
{
    public class ClientRepositoryTests
    {
        [Fact]
        public void Client_CreateNewClient()
        {
            string CPF = "89405690248";
            string name = Utils.RandomString(20);

            Client? client = ClientRepository.Create(CPF, name);

            bool isBalanceZero = client.Account.Balance == 0;
            bool isCpfCorrectLth = client.CPF.Length == 11;

            Assert.NotNull(client);
            Assert.NotNull(client.Account);
            Assert.True(isBalanceZero);
            Assert.True(isCpfCorrectLth);
        }

        [Fact]
        public void Client_Deactivate()
        {
            string CPF = "89405690248";
            string name = Utils.RandomString(20);

            Client? client = ClientRepository.Create(CPF, name);

            Assert.Null(client.DeactivateAt);

            ClientRepository.Deactivate(client);

            Assert.NotNull(client.DeactivateAt);
        }

        [Fact]
        public void Client_UpdateName()
        {
            string CPF = "89405690248";
            string name = Utils.RandomString(20);

            Client? client = ClientRepository.Create(CPF, name);

            Assert.True(string.Equals(client.Name, name));

            string newName = Utils.RandomString(20);

            ClientRepository.Update(client, newName);

            Assert.True(string.Equals(client.Name, newName));
        }
    }
}