namespace src;

using Bcrypt = BCrypt.Net.BCrypt;

public static class AuthService
{
    private static readonly int BCRYPT_MIN_SALT = 4;
    private static readonly int BCRYPT_MAX_SALT = 15;
    public static string HashPassword(string password, int salt) =>
        Bcrypt.HashPassword(password, salt);

    public static bool ComparePassword(string password, string hash) =>
        Bcrypt.Verify(password, hash);

    public static int RandomSalt() =>
        Utils.RandomInt(BCRYPT_MIN_SALT, BCRYPT_MAX_SALT);
}
