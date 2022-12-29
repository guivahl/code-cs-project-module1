namespace src;

using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

using System.Reflection;
public class FileService
{
    private string Filepath { get; init; }
    private CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture) {
        
    };
    public FileService(string filename)
    {
        string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        string filepath = Path.Join(pathDesktop, filename);

        this.Filepath = filepath;
    }
    public bool FileExists() => new FileInfo(this.Filepath).Exists;

    public List<T> Read<T>()
    {
        using StreamReader stream = new StreamReader(this.Filepath);
        using CsvReader csv = new CsvReader(stream, CultureInfo.InvariantCulture);

        List<T> records = csv.GetRecords<T>().ToList();

        return records;
    }

    public void Write<T>(List<T> records)
    {
        using StreamWriter stream = new StreamWriter(this.Filepath);
        using CsvWriter csv = new CsvWriter(stream, CultureInfo.InvariantCulture);

        csv.WriteRecords(records);
   }
}
