namespace src;
class Program
{
    static void Main(string[] args)
    {
        ClientRepository.Load();
        EmployeeRepository.Load();

        while(!EmployeeService.Authenticate()) {
            System.Console.WriteLine("Authentication failed!");
            System.Console.WriteLine("Press any key to try again!");
                    
            Console.ReadKey();
            Console.Clear();
        };
    }
}
