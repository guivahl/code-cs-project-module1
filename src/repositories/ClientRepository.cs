namespace src;

using DanielMarques.Utilities;

public static class ClientRepository
{
    private static readonly int CPF_LENGTH = 11;
    private static readonly string CSV_FILENAME = "clients.csv";

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

        ClientRepository.Save();

        return client;
    }

    public static void Show(Client client) {
        string clientInfo = $"Cliente: {client.Name}\nCPF: {client.CPF}\nSaldo: {client.Account.Balance}\n";

        string accountInfo = $"Conta: {client.Account.AccountNumber}-{client.Account.CheckDigit}\nAgência: {client.Account.Agency}-{client.Account.CheckDigit}\n";
        System.Console.WriteLine(String.Join("", clientInfo, accountInfo));
    }
    public static void ShowAll() => Clients.ForEach(client => ClientRepository.Show(client));
    
    public static void Update(Client client, string newName) {
        client.Name = newName;

        ClientRepository.Save();
    }
    public static void Deactivate(Client client) {
        client.DeactivateAt = DateTime.Now;

        ClientRepository.Save();
    }

    private static void Save() {
        FileService clientFile = new FileService(ClientRepository.CSV_FILENAME);

        List<ClientAccountDto> clientsAccounts = new List<ClientAccountDto>();

        Clients.ForEach(client => clientsAccounts.Add(new ClientAccountDto(client, client.Account)));

        clientFile.Write(clientsAccounts);
    }

    public static void Load() {
        FileService clientFile = new FileService(ClientRepository.CSV_FILENAME);

        List<ClientAccountDto> clientsAccounts = clientFile.Read<ClientAccountDto>();

        Clients = clientsAccounts.Select(clientAccount => new Client(clientAccount)).ToList();
    }
}