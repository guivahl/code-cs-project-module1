namespace src;

using Bcrypt = BCrypt.Net.BCrypt;

public static class AuthService
{
    public static string HashPassword(string password, int salt) =>
        Bcrypt.HashPassword(password, salt);
    
    public static bool ComparePassword(string password, string hash) => 
        Bcrypt.Verify(password, hash);
}
