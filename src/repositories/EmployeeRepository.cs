namespace src;

public static class EmployeeRepository
{
    private static readonly string CSV_FILENAME = "employees.csv";
    private static List<Employee> Employees { get; set; } = new List<Employee>();

    public static Employee? FindByUsername(string? username) =>
        Employees.FirstOrDefault(employee => employee.Username == username);

    public static bool HasActiveEmployees() =>
        Employees.Any(employee =>
            employee.DeactivateAt == null && employee.LastLoginAt != null
        );

    public static void UpdateLastLogin(Employee employee)
    {
        employee.LastLoginAt = DateTime.Now;

        EmployeeRepository.Save();
    }
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

        EmployeeRepository.Save();

        return employee;
    }

    public static void UpdatePassword(Employee employee, string password, int passwordSalt)
    {
        employee.SetPassword(password, passwordSalt);

        EmployeeRepository.Save();
    }

    public static bool IsDeactivate(Employee employee) => employee.DeactivateAt != null;

    public static void Deactivate(Employee employee)
    {
        employee.DeactivateAt = DateTime.Now;

        EmployeeRepository.Save();
    }
    private static void Save()
    {
        FileService employeeFile = new FileService(EmployeeRepository.CSV_FILENAME);

        employeeFile.Write(Employees);
    }

    public static void Load()
    {
        FileService employeeFile = new FileService(EmployeeRepository.CSV_FILENAME);

        employeeFile.CreateFileIfNotExists();
        
        Employees = employeeFile.Read<Employee>();
    }
    public static bool HasEmployeesRegistered() => Employees.Count != 0;

    public static List<Employee> ActiveEmployees() =>
        Employees.Where(employee => employee.DeactivateAt == null && employee.LastLoginAt != null).ToList();
}