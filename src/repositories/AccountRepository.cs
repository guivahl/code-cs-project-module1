namespace src;

public static class AccountRepository
{
    private static readonly int ACCOUNT_NUMBER_LENGTH = 5;
    private static readonly int CHECK_DIGIT_LENGTH = 1;
    private static List<Account> Accounts { get; } = new List<Account>();

    private static bool AlreadyCreated(string accountNumber) => 
        Accounts.Any(account => account.AccountNumber == accountNumber);

    public static Account? Create() {
        int tries = 0, MAX_TRIES = 10;

        string accountNumber = Utils.RandomStringOfNumbers(ACCOUNT_NUMBER_LENGTH);
        string checkDigit = Utils.RandomStringOfNumbers(CHECK_DIGIT_LENGTH);

        while (AlreadyCreated(accountNumber) && tries < MAX_TRIES) {
            accountNumber = Utils.RandomStringOfNumbers(ACCOUNT_NUMBER_LENGTH);        
            tries++;
        }

        if (tries == MAX_TRIES) {
            System.Console.WriteLine("Not possible to create new account");
            return null;
        }
        
        Account account = new Account(accountNumber, checkDigit);
        
        Accounts.Add(account);

        return account;
    }

    public static Account Create(ClientAccountDto clientAccount) {
        Account account = new Account(clientAccount);
        
        Accounts.Add(account);

        return account;
    }
    
    public static void ShowAll () => Accounts.ForEach(account => System.Console.WriteLine(account.AccountNumber)); 

    public static Account? Find(string accountNumber) => 
        Accounts.FirstOrDefault(account => account.AccountNumber == accountNumber);
}
