namespace src;

using GetPass;

public class EmployeeService
{
    public static bool Authenticate()
    {
        bool isFirstAccess = EmployeeService.IsFirstAccess();

        if (isFirstAccess)
        {
            return EmployeeService.FirstAccess();
        }

        return EmployeeService.Login();
    }
    public static bool IsFirstAccess()
    {
        bool hasEmployeeRegistered = EmployeeRepository.HasEmployeesRegistered();

        if (!hasEmployeeRegistered) return true;

        bool hasActiveEmployees = EmployeeRepository.HasActiveEmployees();

        return !hasActiveEmployees;
    }

    public static void CreateFirstUser()
    {
        string firstUser = "user";
        string initialPassword = "pass";

        EmployeeRepository.Create(firstUser, initialPassword);
    }

    public static bool FirstAccess()
    {
        string firstUser = "user";
        string initialPassword = "pass";

        Employee? employee = EmployeeRepository.Create(firstUser, initialPassword);

        if (employee == null)
        {
            System.Console.WriteLine("Error creating first user");
            return false;
        }

        while (!EmployeeService.Login())
        {
            System.Console.WriteLine("Login failed!");
            System.Console.WriteLine("Press any key to try again!");

            Console.ReadKey();
            Console.Clear();
        };

        EmployeeService.UpdatePassword(firstUser);

        return true;
    }
    public static bool Login()
    {
        System.Console.Write("Username: ");
        string? username = Console.ReadLine();

        string password = ConsolePasswordReader.Read();

        Employee? employee = EmployeeRepository.FindByUsername(username);

        if (employee == null)
        {
            System.Console.WriteLine("User not found");
            Console.ReadKey();
            Console.Clear();
            return false;
        }

        if (EmployeeRepository.IsDeactivate(employee))
        {
            System.Console.WriteLine("User not activated");
            Console.ReadKey();
            Console.Clear();
            return false;
        }

        bool isPasswordCorrect = AuthService.ComparePassword(password, employee.Password);

        if (!isPasswordCorrect)
        {
            System.Console.WriteLine("Password incorrect! Press enter to continue...");
            Console.ReadKey();
            Console.Clear();
            return false;
        }

        EmployeeRepository.UpdateLastLogin(employee);

        return true;
    }
    public static void UpdatePassword(string? username)
    {
        Employee? employee = EmployeeRepository.FindByUsername(username);

        if (employee == null)
        {
            System.Console.WriteLine("User not found");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        string password = ConsolePasswordReader.Read("New password: ");

        int passwordSalt = AuthService.RandomSalt();

        string hashedPassword = AuthService.HashPassword(password, passwordSalt);

        EmployeeRepository.UpdatePassword(employee, hashedPassword, passwordSalt);

    }
    public static void UpdatePassword()
    {
        System.Console.Write("Username: ");

        string? username = Console.ReadLine();

        EmployeeService.UpdatePassword(username);
    }
    public static void Deactivate()
    {
        System.Console.Write("Username: ");

        string? username = Console.ReadLine();

        Employee? employee = EmployeeRepository.FindByUsername(username);

        if (employee == null)
        {
            System.Console.WriteLine("User not found");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        EmployeeRepository.Deactivate(employee);
        
        System.Console.WriteLine("Successful deactivate");

        Console.ReadKey();
        Console.Clear();
    }
    public static void Create()
    {
        System.Console.Write("Username: ");

        string? username = Console.ReadLine();

        if (string.IsNullOrEmpty(username))
        {
            System.Console.WriteLine("Username should not be empty");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        Employee? alreadyRegistered = EmployeeRepository.FindByUsername(username);

        if (alreadyRegistered != null)
        {
            System.Console.WriteLine("Username registered");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        string password = ConsolePasswordReader.Read();

        Employee? employee = EmployeeRepository.Create(username, password);

        if (employee != null)
        {
            System.Console.WriteLine("Employee registered");
            Console.ReadKey();
            Console.Clear();

            return;
        }
    }

};