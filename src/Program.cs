namespace src;
using ConsoleTools;

class Program
{
    static void Main(string[] args)
    {
        ClientRepository.Load();
        EmployeeRepository.Load();

        while (!EmployeeService.Authenticate())
        {
            System.Console.WriteLine("Authentication failed!");
            System.Console.WriteLine("Press any key to try again!");

            Console.ReadKey();
            Console.Clear();
        };

        ConsoleMenu clientMenu = new ConsoleMenu()
            .Add("Create new client", () => ClientService.Create())
            .Add("Search client", () => ClientService.Find())
            .Add("Update client", () => ClientService.Update())
            .Add("Deactivate client", () => ClientService.Deactivate())
            .Add("Close", ConsoleMenu.Close)
            .Configure(config =>
                    {
                        config.Selector = ">>> ";
                        config.Title = "Client Menu";
                        config.EnableWriteTitle = true;
                        config.EnableBreadcrumb = true;
                    });

        ConsoleMenu employeeMenu = new ConsoleMenu()
            .Add("Create new employee", () => EmployeeService.Create())
            .Add("Update employee password", () => EmployeeService.UpdatePassword())
            .Add("Deactivate employee", () => EmployeeService.Deactivate())
            .Add("Close", ConsoleMenu.Close)
            .Configure(config =>
                    {
                        config.Selector = ">>> ";
                        config.Title = "Employee Menu";
                        config.EnableWriteTitle = true;
                        config.EnableBreadcrumb = true;
                    });

        ConsoleMenu reportMenu = new ConsoleMenu()
            .Add("Show active clients", () => ReportService.ShowActiveClients())
            .Add("Show inactive clients", () => ReportService.ShowInactiveClients())
            .Add("Show active employees", () => ReportService.ShowActiveEmployees())
            .Add("Show transactions with errors", () => ReportService.ShowTransactionsFailed())
            .Add("Close", ConsoleMenu.Close)
            .Configure(config =>
                    {
                        config.Selector = ">>> ";
                        config.Title = "Report Menu";
                        config.EnableWriteTitle = true;
                        config.EnableBreadcrumb = true;
                    });

        ConsoleMenu transactionMenu = new ConsoleMenu()
            .Add("Process transactions", () => TransactionService.ProcessTransactions())
            .Add("Close", ConsoleMenu.Close)
            .Configure(config =>
                    {
                        config.Selector = ">>> ";
                        config.Title = "Transaction Menu";
                        config.EnableWriteTitle = true;
                        config.EnableBreadcrumb = true;
                    });

        ConsoleMenu menu = new ConsoleMenu()
            .Add("Clients", clientMenu.Show)
            .Add("Employees", employeeMenu.Show)
            .Add("Reports", reportMenu.Show)
            .Add("Transactions", transactionMenu.Show)
            .Add("Close", ConsoleMenu.Close)
            .Configure(config =>
                    {
                        config.Selector = ">>> ";
                        config.Title = "Main Menu";
                        config.EnableWriteTitle = true;
                        config.EnableBreadcrumb = true;
                    });

        menu.Show();
    }
}
