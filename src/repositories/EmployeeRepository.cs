namespace src;

public static class EmployeeRepository
{
    private static List<Employee> Employees { get; } = new List<Employee>();
    
    private static Employee? FindByUsername(string username) =>
        Employees.FirstOrDefault(employee => employee.Username == username);

    private static void UpdateLastLogin(Employee employee) => employee.LastLoginAt = DateTime.Now;
    
    public static Employee? Create(string username, string password)
    {
        Employee? alreadyRegistered = EmployeeRepository.FindByUsername(username);

        if (alreadyRegistered != null)
        {
            System.Console.WriteLine("Employee already registered");
            return null;
        }

        int passwordSalt = AuthService.RandomSalt();

        string hashedPassword = AuthService.HashPassword(password, passwordSalt);

        Employee employee = new Employee(username, hashedPassword, passwordSalt);

        Employees.Add(employee);

        return employee;
    }

    public static void UpdatePassword(Employee employee, string password)
    {
        int passwordSalt = AuthService.RandomSalt();

        string hashedPassword = AuthService.HashPassword(password, passwordSalt);

        employee.SetPassword(hashedPassword, passwordSalt);
    }

    public static bool Login (string username, string password) {
        Employee? employee = EmployeeRepository.FindByUsername(username);

        if (employee == null) {
            System.Console.WriteLine("User not found");
            return false;
        }

        bool isPasswordCorrect = AuthService.ComparePassword(password, employee.Password);

        if (!isPasswordCorrect) {
            System.Console.WriteLine("Password incorrect");
            return false;
        }

        return true;
    }

    public static void Deactivate(Employee employee) => employee.DeactivateAt = DateTime.Now;
}