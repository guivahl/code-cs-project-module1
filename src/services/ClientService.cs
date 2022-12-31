namespace src;

using DanielMarques.Utilities;

public class ClientService
{
    private static readonly int CPF_LENGTH = 11;

    public static Client? Create()
    {
        System.Console.Write("Name: ");
        string? name = Console.ReadLine();

        System.Console.Write("CPF: ");
        string? clientCpf = Console.ReadLine();

        if (string.IsNullOrEmpty(clientCpf) || string.IsNullOrEmpty(name))
        {
            System.Console.WriteLine("Name and CPF should not be empty");
            Console.ReadKey();
            Console.Clear();
            return null;
        }

        if (clientCpf.Length != CPF_LENGTH)
        {
            System.Console.WriteLine("CPF must have 11 numeric digits");
            Console.ReadKey();
            Console.Clear();
            return null;
        }

        bool isValidCpf = CPF.IsValid(clientCpf);

        if (!isValidCpf)
        {
            System.Console.WriteLine("Invalid CPF");
            Console.ReadKey();
            Console.Clear();
            return null;
        }

        Client? alreadyRegistered = ClientRepository.FindByCpf(clientCpf);

        if (alreadyRegistered != null)
        {
            System.Console.WriteLine("Client already registered");
            Console.ReadKey();
            Console.Clear();
            return null;
        }

        Account? account = AccountRepository.Create();

        if (account == null) return null;

        Client client = ClientRepository.Create(clientCpf, name, account);

        return client;
    }

    public static void Find()
    {
        System.Console.Write("CPF: ");
        string? clientCpf = Console.ReadLine();

        if (string.IsNullOrEmpty(clientCpf))
        {
            System.Console.WriteLine("CPF should not be empty");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        Client? client = ClientRepository.FindByCpf(clientCpf);

        if (client == null)
        {
            System.Console.WriteLine("User not found");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        ClientRepository.Show(client);

        Console.ReadKey();
        Console.Clear();
    }

     public static void Update()
    {
        System.Console.Write("CPF: ");
        string? clientCpf = Console.ReadLine();

        if (string.IsNullOrEmpty(clientCpf))
        {
            System.Console.WriteLine("CPF should not be empty");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        Client? client = ClientRepository.FindByCpf(clientCpf);

        if (client == null)
        {
            System.Console.WriteLine("User not found");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        System.Console.Write("New name: ");
        string? name = Console.ReadLine();

        if (string.IsNullOrEmpty(name))
        {
            System.Console.WriteLine("Name should not be empty");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        ClientRepository.Update(client, name);

        System.Console.WriteLine("Successful update");

        Console.ReadKey();
        Console.Clear();
    }

     public static void Deactivate()
    {
        System.Console.Write("CPF: ");
        string? clientCpf = Console.ReadLine();

        if (string.IsNullOrEmpty(clientCpf))
        {
            System.Console.WriteLine("CPF should not be empty");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        Client? client = ClientRepository.FindByCpf(clientCpf);

        if (client == null)
        {
            System.Console.WriteLine("User not found");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        ClientRepository.Deactivate(client);

        System.Console.WriteLine("Successful deactivate");

        Console.ReadKey();
        Console.Clear();
    }
}
