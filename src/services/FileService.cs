namespace src;

using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

public class FileService
{
    private string Filepath { get; init; }
    private string? Folder { get; init; }
    private CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture);
    public FileService(string fileName, string? folder = null)
    {
        string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string folderPath = Path.Join(pathDesktop, folder);
        string filePath = Path.Join(folderPath, fileName);

        this.Filepath = filePath;
        this.Folder = folderPath;
    }
    public bool FileExists() => new FileInfo(this.Filepath).Exists;

    public void CreateFolderIfNotExists()
    {
        if (this.Folder == null) return;

        DirectoryInfo target = new DirectoryInfo(this.Folder);

        if (target.Exists) return;

        target.Create();
    }
    public static void CreateFolderIfNotExists(string folderPath)
    {
        DirectoryInfo target = new DirectoryInfo(folderPath);

        if (target.Exists) return;

        target.Create();
    }

    public List<T> Read<T, TMap>(bool hasHeader = false)
    where TMap : ClassMap
    {

        CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = hasHeader,
            MissingFieldFound = null
        };

        using var reader = new StreamReader(this.Filepath);
        using var csv = new CsvReader(reader, config);

        csv.Context.RegisterClassMap<TMap>();

        return csv.GetRecords<T>().ToList();
    }
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
