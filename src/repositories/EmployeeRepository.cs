namespace src;

public static class EmployeeRepository
{
    private static List<Employee> Employees { get; } = new List<Employee>();
    
    private static Employee? FindByUsername(string username) =>
        Employees.FirstOrDefault(employee => employee.Username == username);

    private static void UpdateLastLogin(Employee employee) => employee.LastLoginAt = new DateTime();
    
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

    public static void Deactivate(Employee employee) => employee.DeactivateAt = new DateTime();
}