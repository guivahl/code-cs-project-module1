namespace Entities.Tests
{
    public class EmployeeTests
    {
        [Fact]
        public void Employee_CreateNewEmployee()
        {
            string username = Utils.RandomStringOfNumbers(6);
            string password = Utils.RandomStringOfNumbers(10);
            int passwordSalt = Utils.RandomInt(5, 10);

            Employee employee = new Employee(username, password, passwordSalt);

            Assert.Null(employee.LastLoginAt);
            Assert.Null(employee.DeactivateAt);
            Assert.True(password == employee.Password);
            Assert.True(passwordSalt == employee.PasswordSalt);
            Assert.True(username == employee.Username);
        }

        [Fact]
        public void Employee_SetPassword()
        {
            string username = Utils.RandomStringOfNumbers(6);
            string password = Utils.RandomStringOfNumbers(10);
            int passwordSalt = Utils.RandomInt(5, 10);

            Employee employee = new Employee(username, password, passwordSalt);

            string newPassword = Utils.RandomStringOfNumbers(10);
            int newPasswordSalt = Utils.RandomInt(5, 10);

            employee.SetPassword(newPassword, newPasswordSalt);

            Assert.True(newPassword == employee.Password);
            Assert.True(newPasswordSalt == employee.PasswordSalt);
        }
    }
}