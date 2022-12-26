namespace AuthServiceTests
{
    public class AuthServiceTests
    {
        [Fact]
        public void AuthService_HashPassword()
        {
            string password = "password";
            int salt = 10;

            string hashedPassword = AuthService.HashPassword(password, salt);

            Assert.IsType<string>(hashedPassword);
        }

        [Fact]
        public void AuthService_ComparePassword()
        {
            string password = "password";
            int salt = 10;

            string hashedPassword = AuthService.HashPassword(password, salt);

            bool isPasswordEqual = AuthService.ComparePassword(password, hashedPassword);

            Assert.True(isPasswordEqual);
        }
        
    }
}