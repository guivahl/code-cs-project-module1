namespace src;

public static class EmployeeRepository
{
    private static readonly string CSV_FILENAME = "employees.csv";
    private static List<Employee> Employees { get; set; } = new List<Employee>();
    
    private static Employee? FindByUsername(string username) =>
        Employees.FirstOrDefault(employee => employee.Username == username);

    private static void UpdateLastLogin(Employee employee) {
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

    public static void UpdatePassword(Employee employee, string password)
    {
        int passwordSalt = AuthService.RandomSalt();

        string hashedPassword = AuthService.HashPassword(password, passwordSalt);

        employee.SetPassword(hashedPassword, passwordSalt);

        EmployeeRepository.Save();
    }

    public static bool Login (string username, string password) {
        Employee? employee = EmployeeRepository.FindByUsername(username);

        if (employee == null) {
            System.Console.WriteLine("User not found");
            return false;
        }

        if (EmployeeRepository.IsDeactivate(employee)) {
            System.Console.WriteLine("User not activated");
            return false;
        }

        bool isPasswordCorrect = AuthService.ComparePassword(password, employee.Password);

        if (!isPasswordCorrect) {
            System.Console.WriteLine("Password incorrect");
            return false;
        }

        EmployeeRepository.UpdateLastLogin(employee);

        return true;
    }

    private static bool IsDeactivate(Employee employee) => employee.DeactivateAt != null;

    public static void Deactivate(Employee employee) {
        employee.DeactivateAt = DateTime.Now;

        EmployeeRepository.Save();
    }
    private static void Save() {
        FileService employeeFile = new FileService(EmployeeRepository.CSV_FILENAME);

        employeeFile.Write(Employees);
    }

    public static void Load() {
        FileService employeeFile = new FileService(EmployeeRepository.CSV_FILENAME);

        Employees = employeeFile.Read<Employee>();
    }
}