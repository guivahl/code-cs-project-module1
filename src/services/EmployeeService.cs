namespace src;

using GetPass;

public class EmployeeService
{
    public static bool Authenticate () {
        bool isFirstAccess = EmployeeService.IsFirstAccess();

        if (isFirstAccess) {
            return EmployeeService.FirstAccess();
        }

        return EmployeeService.Login();
    }
    public static bool IsFirstAccess() {
        bool hasEmployeeRegistered = EmployeeRepository.HasEmployeesRegistered();

        if (!hasEmployeeRegistered) return true;

        bool hasActiveEmployees = EmployeeRepository.HasActiveEmployees();

        return !hasActiveEmployees;
    }

    public static void CreateFirstUser() {
        string firstUser = "user";
        string initialPassword = "pass";

        EmployeeRepository.Create(firstUser, initialPassword);
    }

    public static bool FirstAccess() {
        string firstUser = "user";
        string initialPassword = "pass";

        Employee? employee = EmployeeRepository.Create(firstUser, initialPassword);
    
        if (employee == null) {
            System.Console.WriteLine("Error creating first user");
            return false;
        }
        
        while(!EmployeeService.Login()) {
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
            return false;
        }

        if (EmployeeRepository.IsDeactivate(employee))
        {
            System.Console.WriteLine("User not activated");
            return false;
        }

        bool isPasswordCorrect = AuthService.ComparePassword(password, employee.Password);

        if (!isPasswordCorrect)
        {
            System.Console.WriteLine("Password incorrect");
            return false;
        }

        EmployeeRepository.UpdateLastLogin(employee);

        return true;
    }
    public static void UpdatePassword(string username) {
        Employee? employee = EmployeeRepository.FindByUsername(username);

        if (employee == null) {
            System.Console.WriteLine("User not found");
            return;
        }

        string password = ConsolePasswordReader.Read("New password: ");

        int passwordSalt = AuthService.RandomSalt();

        string hashedPassword = AuthService.HashPassword(password, passwordSalt);

        EmployeeRepository.UpdatePassword(employee, hashedPassword, passwordSalt);

    }

};