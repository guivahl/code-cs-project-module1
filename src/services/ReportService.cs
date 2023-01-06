namespace src;

using BetterConsoles.Tables.Configuration;
using BetterConsoles.Tables;

public class ReportService
{
    private static void Show(string[] columns, IEnumerable<object?[]> data)
    {
        Table table = new Table(TableConfig.MySql(), columns);

        table.AddRows(data);

        Console.Write(table.ToString());
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
        Console.ReadKey();
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
        Console.ReadKey();

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
        Console.ReadKey();
    }

    public static void ShowTransactionsFailed()
    {
        FileInfo[] transactionFiles = TransactionService.TransactionFiles(TransactionService.FOLDER_FAILED);

        string[] originColumns = new string[] {
            "OriginBankCode", "OriginAgency", "OriginAccountNumber"
        };
        string[] destinationColumns = new string[] {
            "DestinationBankCode", "DestinationAgency", "DestinationAccountNumber"
        };
        string[] operationDetails = new string[] {
            "TransactionType","TransactionWay","Value", "ErrorMessage"
        };

        if (transactionFiles.Length == 0) {
            System.Console.WriteLine("Failed transactions not found! Press enter to continue...");
            Console.ReadKey();

            return;
        }

        System.Console.WriteLine($"Number of failed transactions file found: {transactionFiles.Length}");

        for (int i = 0; i < transactionFiles.Length; i++)
        {
            string inputFileName = transactionFiles[i].Name;
            System.Console.WriteLine($"File: {inputFileName}");

            FileService file = new FileService(inputFileName, TransactionService.FOLDER_FAILED);
            List<TransactionDto> failedTransactions = file.Read<TransactionDto, TransactionMapRead>(true);

            IEnumerable<object?[]> origin = failedTransactions
                .Select(transaction => new object?[] {
                    transaction.OriginBankCode,
                    transaction.OriginAgency,
                    transaction.OriginAccountNumber
                });

            IEnumerable<object?[]> destination = failedTransactions
                .Select(transaction => new object?[] {
                    transaction.DestinationBankCode,
                    transaction.DestinationAgency,
                    transaction.DestinationAccountNumber
                });


            IEnumerable<object?[]> details = failedTransactions
                .Select(transaction => new object?[] {
                    transaction.TransactionType.ToString(),
                    transaction.TransactionWay.ToString(),
                    transaction.Value,
                    transaction.ErrorMessage
                });

            System.Console.WriteLine("#### ORIGIN ACCOUNT ####");
            ReportService.Show(originColumns, origin);

            System.Console.WriteLine("#### DESTINATION ACCOUNT ####");
            ReportService.Show(destinationColumns, destination);

            System.Console.WriteLine("#### OPERATION FAIL DETAILS ####");
            ReportService.Show(operationDetails, details);

            System.Console.WriteLine("Press Enter to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
