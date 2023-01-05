namespace src;
/**
Essas falhas podem acontecer, por exemplo, 
por insuficiência de saldo, número da conta inválido 
ou inexistente, tipo de transação incompatível 
(no caso de TEFs), etc.
Nesses casos, o registro da transação deve ser movido 
para um arquivo cujo padrão de nomenclatura
 é "nome-do-banco-parceiro-aaaammdd-failed.csv" que 
 deve ser armazenado na pasta "~/home/Transactions/Failed".

Os motivos no caso seriam saldo insuficiente, 
dados incorretos, e tipo de transação incompatível?
*/

public class TransactionService
{
    // input Desktop/Transactions
    // nome-do-banco-parceiro-aaaammdd.csv

    //falha
    // nome-do-banco-parceiro-aaaammdd-failed.csv

    // sucesso 
    // nome-do-banco-parceiro-aaaammdd-completed.csv
    private static readonly string folder = "Transactions";
    private static readonly string folderFailed = Path.Join(folder, "Failed");
    private static readonly string folderCompleted = Path.Join(folder, "Completed");
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
        string pathTransaction = Path.Join(pathDesktop, folder);

        DirectoryInfo transactionFolder = new DirectoryInfo(pathTransaction);

        FileService.CreateFolderIfNotExists(pathTransaction);

        FileInfo[] transactionFiles = transactionFolder.GetFiles();

        return transactionFiles;
    }

    private static bool IsValidTef(TransactionDto transaction) =>
        transaction.OriginBankCode == transaction.DestinationBankCode;

    private static bool IsValidClientAndAccount(TransactionDto transaction)
    {
        Account? account = null;

        if (transaction.TransactionWay == TransactionWay.Débito)
        {
            account = AccountRepository.Find(transaction.OriginBankCode, transaction.OriginAgency, transaction.OriginAccountNumber);
        }

        if (transaction.TransactionWay == TransactionWay.Crédito)
        {
            account = AccountRepository.Find(transaction.DestinationBankCode, transaction.DestinationAgency, transaction.DestinationAccountNumber);
        }

        if (account == null) return false;

        Client? client = ClientRepository.FindByAccount(account);

        if (client == null) return false;

        if (ClientRepository.IsActiveClient(client)) return false;

        if (transaction.TransactionWay == TransactionWay.Débito)
        {
            bool hasEnoughBalance = ClientRepository.ValidClientBalance(client, transaction.Value);

            if (!hasEnoughBalance) return false;
        }

        return true;
    }

    private static TransactionDto ValidateTransaction(TransactionDto transaction)
    {
        List<string> errors = new List<string>();

        if (transaction.TransactionType == TransactionType.TEF)
        {
            if (!TransactionService.IsValidTef(transaction))
                errors.Add("TEF's cannot be done with clients from differente banks");
        }

        if (!TransactionService.IsValidClientAndAccount(transaction))
        {
            errors.Add("Account or client invalid");
        }

        if (errors.Count() > 0)
        {
            transaction.ErrorMessage = String.Join("; ", errors);
        }

        return transaction;
    }

    private static List<TransactionDto> ValidateTransactions(List<TransactionDto> transactions) =>
    transactions.Select(
            transaction =>
            TransactionService.ValidateTransaction(transaction)
        ).ToList();

    private static void SaveCompletedTransactions(List<TransactionDto> transactions, string originalFileName)
    {
        string completedPath = TransactionService.CompletedTransactionFileName(originalFileName);

        FileService file = new FileService(completedPath, folderCompleted);

        file.CreateFolderIfNotExists();

        file.Write(transactions);
    }
    private static void SaveFailedTransactions(List<TransactionDto> transactions, string originalFileName)
    {
        string failedPath = TransactionService.FailedTransactionFileName(originalFileName);
    
        FileService file = new FileService(failedPath, folderFailed);

        file.CreateFolderIfNotExists();

        file.Write(transactions);
    }

    private static void ProcessFile(FileInfo transactionFile)
    {
        string inputFileName = transactionFile.Name;

        FileService file = new FileService(inputFileName, folder);
        List<TransactionDto> csvTransactions = file.ReadWithoutHeader<TransactionDto, TransactionMap>();

        List<TransactionDto> transactions = TransactionService.ValidateTransactions(csvTransactions);

        List<TransactionDto> completedTransactions = transactions.Where(transaction => transaction.ErrorMessage == null).ToList();
        List<TransactionDto> failedTransactions = transactions.Where(transaction => transaction.ErrorMessage != null).ToList();

        if (completedTransactions.Count > 0)
        {
            // todo, logica para manipular saldos
            // ver tabela de tarifas
            // se for transferencia do mesmo banco tem que validar
            // tipo ted entre a mesma agencia (que to codando)
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

    }
}

