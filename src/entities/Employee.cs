namespace src;

public class Employee
{
    public string Username { get; set; }
    public string Password { get; private set; }
    public int PasswordSalt { get; private set; }
    public DateTime? LastLoginAt { get; set; }
    public DateTime? DeactivateAt { get; set; }

    public Employee (string username, string password, int passwordSalt) {
        this.Username = username;
        this.Password = password;
        this.PasswordSalt = passwordSalt;
    }

    public void SetPassword(string password, int passwordSalt) {
        this.Password = password;
        this.PasswordSalt = passwordSalt;
    }
}
