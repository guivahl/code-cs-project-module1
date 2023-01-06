namespace src;

public static class ClientRepository
{
    private static readonly string CSV_FILENAME = "clients.csv";

    private static List<Client> Clients { get; set; } = new List<Client>();
    public static Client? FindByCpf(string clientCpf) =>
        Clients.FirstOrDefault(client => client.CPF == clientCpf);

    public static Client? FindByAccount(Account account) =>
        Clients.FirstOrDefault(client => object.Equals(client.Account, account));

    public static Client Create(string clientCpf, string name, Account account)
    {
        Client client = new Client(clientCpf, name, account);

        Clients.Add(client);

        ClientRepository.Save();

        return client;
    }
    public static Client Create(ClientAccountDto clientAccount)
    {
        Account account = AccountRepository.Create(clientAccount);

        Client client = new Client(clientAccount.CPF, clientAccount.Name, account, clientAccount.DeactivateAt);

        Clients.Add(client);

        return client;
    }

    public static void Show(Client client)
    {
        string clientInfo = $"Cliente: {client.Name}\nCPF: {client.CPF}\nSaldo: {client.Account.Balance}\n";

        string accountInfo = $"Conta: {client.Account.AccountNumber}-{client.Account.CheckDigit}\nAgência: {client.Account.Agency}\n";
        System.Console.WriteLine(String.Join("", clientInfo, accountInfo));
    }
    public static void ShowAll() => Clients.ForEach(client => ClientRepository.Show(client));

    public static void Update(Client client, string newName)
    {
        client.Name = newName;

        ClientRepository.Save();
    }
    public static void Deactivate(Client client)
    {
        client.DeactivateAt = DateTime.Now;

        ClientRepository.Save();
    }

    private static void Save()
    {
        FileService clientFile = new FileService(ClientRepository.CSV_FILENAME);

        List<ClientAccountDto> clientsAccounts = new List<ClientAccountDto>();

        Clients.ForEach(client => clientsAccounts.Add(new ClientAccountDto(client, client.Account)));

        clientFile.Write(clientsAccounts);
    }

    public static void Load()
    {
        FileService clientFile = new FileService(ClientRepository.CSV_FILENAME);

        clientFile.CreateFileIfNotExists();

        List<ClientAccountDto> clientsAccounts = clientFile.Read<ClientAccountDto>();

        Clients = clientsAccounts.Select(clientAccount => ClientRepository.Create(clientAccount)).ToList();
    }

    public static List<Client> ActiveClients() =>
        Clients.Where(client => client.DeactivateAt == null).ToList();

    public static List<Client> InactiveClients() =>
        Clients.Where(client => client.DeactivateAt != null).ToList();

    public static bool IsActiveClient(Client client) => client.DeactivateAt != null;

    public static bool ValidClientBalance(Client client, decimal value) =>
        client.Account.Balance > value;

    public static void DebitValue(Client client, decimal value)
    {

        client.Account.DecrementBalance(value);

        ClientRepository.Save();
    }

    public static void CreditValue(Client client, decimal value)
    {

        client.Account.AddBalance(value);

        ClientRepository.Save();
    }
}