namespace src;


using System.Globalization;

public class TransactionService
{
    public static readonly string FOLDER = "Transactions";
    public static readonly string FOLDER_FAILED = Path.Join(TransactionService.FOLDER, "Failed");
    public static readonly string FOLDER_COMPLETED = Path.Join(TransactionService.FOLDER, "Completed");
    private static readonly char SEPARATOR_CHAR_FILE = '-';


    private static string FailedTransactionFileName(string originalFileName)
    {
        string fileExtension = Path.GetExtension(originalFileName);
        string[] fileInfo = Path.GetFileNameWithoutExtension(originalFileName).Split(SEPARATOR_CHAR_FILE);

        string fileDate = fileInfo.Last();
        string bankName = fileInfo.First();
        string fileName = $"{bankName}{SEPARATOR_CHAR_FILE}{fileDate}-failed{fileExtension}";

        return fileName;
    }
    private static string CompletedTransactionFileName(string originalFileName)
    {
        string fileExtension = Path.GetExtension(originalFileName);
        string[] fileInfo = Path.GetFileNameWithoutExtension(originalFileName).Split(SEPARATOR_CHAR_FILE);

        string fileDate = fileInfo.Last();
        string bankName = fileInfo.First();

        string fileName = $"{bankName}{SEPARATOR_CHAR_FILE}{fileDate}-completed{fileExtension}";

        return fileName;
    }

    private static FileInfo[] TransactionFiles()
    {
        string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string pathTransaction = Path.Join(pathDesktop, TransactionService.FOLDER);

        DirectoryInfo transactionFolder = new DirectoryInfo(pathTransaction);

        FileService.CreateFolderIfNotExists(pathTransaction);

        FileInfo[] transactionFiles = transactionFolder.GetFiles();

        return transactionFiles;
    }

    public static FileInfo[] TransactionFiles(string folder)
    {
        string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string pathTransaction = Path.Join(pathDesktop, folder);

        DirectoryInfo transactionFolder = new DirectoryInfo(pathTransaction);

        FileService.CreateFolderIfNotExists(pathTransaction);

        FileInfo[] transactionFiles = transactionFolder.GetFiles();

        return transactionFiles;
    }

    private static bool IsValidTef(TransactionDto transaction) =>
        transaction.OriginBankCode == transaction.DestinationBankCode;

    public static bool IsValidClientAndAccount(string? bankCode, string? agency, string? accountNumberWithDigit)
    {
        Account? account = AccountRepository.Find(bankCode, agency, accountNumberWithDigit);

        if (account == null) return false;

        Client? client = ClientRepository.FindByAccount(account);

        if (client == null) return false;

        if (ClientRepository.IsActiveClient(client)) return false;

        return true;
    }

    private static bool AreValidAccounts(TransactionDto transaction)
    {
        if (
            transaction.OriginBankCode == transaction.DestinationBankCode
        )
        {
            bool isValidOriginAccount = TransactionService.IsValidClientAndAccount(transaction.OriginBankCode, transaction.OriginAgency, transaction.OriginAccountNumber);
            bool isValidDestinationAccount = TransactionService.IsValidClientAndAccount(transaction.OriginBankCode, transaction.OriginAgency, transaction.OriginAccountNumber);

            return isValidOriginAccount && isValidDestinationAccount;
        }

        if (transaction.TransactionWay == TransactionWay.Débito)
        {
            return TransactionService.IsValidClientAndAccount(transaction.OriginBankCode, transaction.OriginAgency, transaction.OriginAccountNumber);
        }

        if (transaction.TransactionWay == TransactionWay.Crédito)
        {
            return TransactionService.IsValidClientAndAccount(transaction.DestinationBankCode, transaction.DestinationAgency, transaction.DestinationAccountNumber);
        }

        return false;
    }

    public static bool HasEnoughBalance(TransactionDto transaction)
    {
        if (transaction.TransactionWay == TransactionWay.Débito)
        {
            Account? account = AccountRepository.Find(transaction.OriginBankCode, transaction.OriginAgency, transaction.OriginAccountNumber);

            if (account == null) return false;

            Client? client = ClientRepository.FindByAccount(account);

            if (client == null) return false;
            bool hasEnoughBalance = ClientRepository.ValidClientBalance(client, transaction.Value);

            return hasEnoughBalance;
        }

        return true;
    }

    private static void ValidateTransaction(TransactionDto transaction)
    {
        List<string> errors = new List<string>();

        if (transaction.TransactionType == TransactionType.TEF)
        {
            if (!TransactionService.IsValidTef(transaction))
                errors.Add("TEF's cannot be done with clients from differente banks");
        }

        if (!TransactionService.AreValidAccounts(transaction))
        {
            errors.Add("Account or client invalid");
        }

        if (!TransactionService.HasEnoughBalance(transaction))
        {
            errors.Add("Account don't have enough balance");
        }

        if (errors.Count() > 0)
        {
            transaction.ErrorMessage = String.Join("; ", errors);
        }
    }

    private static void ValidateTransactions(List<TransactionDto> transactions) =>
    transactions.ForEach(
            transaction =>
            TransactionService.ValidateTransaction(transaction)
        );

    private static void SaveCompletedTransactions(List<TransactionDto> transactions, string originalFileName)
    {
        string completedPath = TransactionService.CompletedTransactionFileName(originalFileName);

        FileService file = new FileService(completedPath, TransactionService.FOLDER_COMPLETED);

        file.CreateFolderIfNotExists();

        file.Write(transactions);
    }
    private static void SaveFailedTransactions(List<TransactionDto> transactions, string originalFileName)
    {
        string failedPath = TransactionService.FailedTransactionFileName(originalFileName);

        FileService file = new FileService(failedPath, TransactionService.FOLDER_FAILED);

        file.CreateFolderIfNotExists();

        file.Write(transactions);
    }
    public static void DebitValue(string? bankCode, string? agency, string? accountNumberWithDigit, decimal debitValue)
    {
        Account? account = AccountRepository.Find(bankCode, agency, accountNumberWithDigit);

        if (account == null) return;

        Client? client = ClientRepository.FindByAccount(account);

        if (client == null) return;

        ClientRepository.DebitValue(client, debitValue);

        return;
    }

    public static void CreditValue(string? bankCode, string? agency, string? accountNumberWithDigit, decimal creditValue)
    {
        Account? account = AccountRepository.Find(bankCode, agency, accountNumberWithDigit);

        if (account == null) return;

        Client? client = ClientRepository.FindByAccount(account);

        if (client == null) return;

        ClientRepository.CreditValue(client, creditValue);

        return;
    }

    public static void UpdateBalance(TransactionDto transaction)
    {
        decimal debitValue = transaction.Value + transaction.Fare;

        if (
            transaction.OriginBankCode == transaction.DestinationBankCode
        )
        {
            TransactionService.DebitValue(transaction.OriginBankCode, transaction.OriginAgency, transaction.OriginAccountNumber, debitValue);
            TransactionService.CreditValue(transaction.DestinationBankCode, transaction.DestinationAgency, transaction.DestinationAccountNumber, transaction.Value);
            return;
        }
        if (transaction.TransactionWay == TransactionWay.Débito)
        {
            TransactionService.DebitValue(transaction.OriginBankCode, transaction.OriginAgency, transaction.OriginAccountNumber, debitValue);
        }

        if (transaction.TransactionWay == TransactionWay.Crédito)
        {
            TransactionService.CreditValue(transaction.DestinationBankCode, transaction.DestinationAgency, transaction.DestinationAccountNumber, transaction.Value);
        }

    }

    public static void UpdateBalances(List<TransactionDto> transactions) =>
    transactions.ForEach(
            transaction =>
            TransactionService.UpdateBalance(transaction)
        );

    public static void ApplyFare(TransactionDto transaction, DateTime fileDate)
    {
        if (transaction.TransactionWay == TransactionWay.Crédito) return;
        if (transaction.OriginBankCode != Account.BANK_CODE && transaction.OriginBankCode != transaction.DestinationBankCode) return;

        if (transaction.TransactionWay == TransactionWay.Débito)
        {
            DateTime debitDateFare = DateTime.ParseExact("20221130", "yyyyMMdd", CultureInfo.InvariantCulture);

            int compareResult = DateTime.Compare(fileDate, debitDateFare);

            bool shouldApplyFare = compareResult > 0;

            if (!shouldApplyFare) return;

            if (transaction.TransactionType == TransactionType.TEF) return;

            if (transaction.TransactionType == TransactionType.TED)
            {
                decimal TEDFare = 5.0m;

                transaction.Fare = TEDFare;
            }
            if (transaction.TransactionType == TransactionType.DOC)
            {
                decimal BASE_DOC_FARE = 1.0m;

                decimal LIMIT_VARIABLE_FARE = 5.0M;
                decimal PERCENTAGE_VARIABLE_FARE = 0.01M;

                decimal formulaVariableFare = PERCENTAGE_VARIABLE_FARE * transaction.Value;

                decimal variableFare = formulaVariableFare < LIMIT_VARIABLE_FARE ? formulaVariableFare : LIMIT_VARIABLE_FARE;

                decimal docFare = BASE_DOC_FARE + variableFare;

                transaction.Fare = docFare;
            }
        };
    }
    public static void ApplyFares(List<TransactionDto> transactions, DateTime fileDate) =>
    transactions.ForEach(
            transaction =>
            TransactionService.ApplyFare(transaction, fileDate)
        );

    private static void ProcessFile(FileInfo transactionFile)
    {
        string inputFileName = transactionFile.Name;

        string fileExtension = Path.GetExtension(inputFileName);
        string[] fileInfo = Path.GetFileNameWithoutExtension(inputFileName).Split(SEPARATOR_CHAR_FILE);

        string fileDate = fileInfo.Last();

        DateTime parsedDate = DateTime.ParseExact(fileDate, "yyyyMMdd", CultureInfo.InvariantCulture);

        FileService file = new FileService(inputFileName, TransactionService.FOLDER);
        List<TransactionDto> transactions = file.Read<TransactionDto, TransactionMapWrite>();

        TransactionService.ApplyFares(transactions, parsedDate);

        TransactionService.ValidateTransactions(transactions);

        List<TransactionDto> completedTransactions = transactions.Where(transaction => transaction.ErrorMessage == null).ToList();
        List<TransactionDto> failedTransactions = transactions.Where(transaction => transaction.ErrorMessage != null).ToList();

        if (completedTransactions.Count > 0)
        {
            TransactionService.UpdateBalances(completedTransactions);
            TransactionService.SaveCompletedTransactions(completedTransactions, inputFileName);
        }
        if (failedTransactions.Count > 0) TransactionService.SaveFailedTransactions(failedTransactions, inputFileName);
    }

    public static void ProcessTransactions()
    {
        FileInfo[] transactionFiles = TransactionService.TransactionFiles();

        for (int i = 0; i < transactionFiles.Length; i++)
        {
            TransactionService.ProcessFile(transactionFiles[i]);
        }
        System.Console.WriteLine("Transactions processed! Press enter to continue...");
        System.Console.ReadKey();
    }
}

