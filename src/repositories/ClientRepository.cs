namespace src;

using DanielMarques.Utilities;

public static class ClientRepository
{
    private static readonly int CPF_LENGTH = 11;
    private static List<Client> Clients { get; set; } = new List<Client>();
    public static Client? FindByCpf(string clientCpf) => 
        Clients.FirstOrDefault(client => client.CPF == clientCpf);

    public static Client? Create(string clientCpf, string name) {
        if (clientCpf.Length != CPF_LENGTH) {
            System.Console.WriteLine("CPF deve possuir 11 digitos numericos");
            return null;
        }

        CPF cpf = clientCpf;
        
        bool isValidCpf = cpf.IsValid();

        if (!isValidCpf) {
            System.Console.WriteLine("CPF inválido");
            return null;
        }

        Client? alreadyRegistered = ClientRepository.FindByCpf(clientCpf);

        if (alreadyRegistered != null) {
            System.Console.WriteLine("Client already registered");
            return null;
        } 

        Account? account = AccountRepository.Create();

        if (account == null) return null;

        Client client = new Client(clientCpf, name, account);

        Clients.Add(client);

        return client;
    }

    public static void Show(Client client) {
        string clientInfo = $"Cliente: {client.Name}\nCPF: {client.CPF}\nSaldo: {client.Account.Balance}\n";

        string accountInfo = $"Conta: {client.Account.AccountNumber}-{client.Account.CheckDigit}\nAgência: {client.Account.Agency}-{client.Account.CheckDigit}\n";
        System.Console.WriteLine(String.Join("", clientInfo, accountInfo));
    }
    
    public static void Update(Client client, string newName) => client.Name = newName;

    public static void Deactivate(Client client) => client.DeactivateAt = new DateTime();
}