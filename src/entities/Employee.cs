namespace src;

public class Employee
{
    public string Username { get; set; }
    public string Password { get; private set; }
    public int PasswordSalt { get; private set; }
    public DateTime? LastLoginAt { get; set; }
    public DateTime? DeactivateAt { get; set; }

    public Employee(string Username, string Password, int PasswordSalt, DateTime? LastLoginAt = null, DateTime? DeactivateAt = null)
    {
        this.Username = Username;
        this.Password = Password;
        this.PasswordSalt = PasswordSalt;
        this.LastLoginAt = LastLoginAt;
        this.DeactivateAt = DeactivateAt;
    }

    public void SetPassword(string password, int passwordSalt)
    {
        this.Password = password;
        this.PasswordSalt = passwordSalt;
    }
}
