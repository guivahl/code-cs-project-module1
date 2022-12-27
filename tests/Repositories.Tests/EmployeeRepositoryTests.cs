namespace Repositories.Tests
{
    public class EmployeeRepositoryTests
    {
        [Fact]
        public void Client_CreateNewEmployee()
        {
            string username = Utils.RandomString(6);
            string password = Utils.RandomString(20);

            Employee? employee = EmployeeRepository.Create(username, password);
        
            bool hasHashedPassword = AuthService.ComparePassword(password, employee.Password);

            Assert.NotNull(employee);
            Assert.True(hasHashedPassword);
        }

        [Fact]
        public void Employee_CreateUpdatePassword()
        {
            string username = Utils.RandomString(6);
            string password = Utils.RandomString(20);

            Employee? employee = EmployeeRepository.Create(username, password);
        
            string newPassword = Utils.RandomString(20);

            EmployeeRepository.UpdatePassword(employee, newPassword);

            bool hasUpdatedPassword = AuthService.ComparePassword(newPassword, employee.Password);

            Assert.True(hasUpdatedPassword);
        }
        
        [Fact]
        public void Employee_Deactivate()
        {   
            string username = Utils.RandomString(6);
            string password = Utils.RandomString(20);

            Employee? employee = EmployeeRepository.Create(username, password);

            Assert.Null(employee.DeactivateAt);

            EmployeeRepository.Deactivate(employee);

            Assert.NotNull(employee.DeactivateAt);
        }
    }
}