namespace src;

using BetterConsoles.Tables.Configuration;
using BetterConsoles.Tables;

public class ReportService
{
    private static void Show(string[] columns, IEnumerable<object?[]> data) {
        Table table = new Table(TableConfig.MySql(), columns);

        table.AddRows(data);

        Console.Write(table.ToString());
        Console.ReadKey();
    }
    public static void ShowActiveClients()
    {
        List<Client> activeClients = ClientRepository.ActiveClients();

        string[] columns = new string[] {
            "Name", "CPF", "Balance (R$)"
        };
        IEnumerable<string[]> clients = activeClients
            .Select(client => new string[] {
                client.Name, client.CPF, client.Account.Balance.ToString()
            });

        ReportService.Show(columns, clients);
    }
    public static void ShowInactiveClients()
    {
        List<Client> inactiveClients = ClientRepository.InactiveClients();

        string[] columns = new string[] {
            "Name", "CPF", "DeactiveAt (mm/dd/yyyy)"
        };
        IEnumerable<object?[]> clients = inactiveClients
            .Select(client => new object?[] {
                client.Name, client.CPF, 
                client.DeactivateAt
            });
            
        ReportService.Show(columns, clients);

    }
    public static void ShowActiveEmployees()
    {
        List<Employee> activeEmployees = EmployeeRepository.ActiveEmployees();

        string[] columns = new string[] {
            "Username", "LastLoginAt (mm/dd/yyyy)"
        };

        IEnumerable<object?[]> employees = activeEmployees
            .Select(employee => new object?[] {
                employee.Username, employee.LastLoginAt
            });

        ReportService.Show(columns, employees);
    }

    // To-do : Exibir Transações com Erro (Detalhes da transação e do Erro)
    public static void ShowTransactionsFailed() { }
}
