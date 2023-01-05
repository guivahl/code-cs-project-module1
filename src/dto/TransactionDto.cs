namespace src;

using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
public class TransactionDto
{
    [Index(0)]
    public string? OriginBankCode { get; set; }

    [Index(1)]
    public string? OriginAgency { get; set; }

    [Index(2)]
    public string? OriginAccountNumber { get; set; }

    [Index(3)]
    public string? DestinationBankCode { get; set; }

    [Index(4)]
    public string? DestinationAgency { get; set; }

    [Index(5)]
    public string? DestinationAccountNumber { get; set; }

    [Index(6)]
    public TransactionType TransactionType { get; set; }

    [Index(7)]
    public TransactionWay TransactionWay { get; set; }
    [Index(8)]
    public decimal Value { get; set; }

    [Index(9)]
    public string? ErrorMessage { get; set; }

    [Index(8)]
    public decimal Fare { get; set; }
}
public sealed class TransactionMap : ClassMap<TransactionDto>
{
    public TransactionMap()
    {
        Map(f => f.OriginBankCode).Index(0);
        Map(f => f.OriginAgency).Index(1);
        Map(f => f.OriginAccountNumber).Index(2);
        Map(f => f.DestinationBankCode).Index(3);
        Map(f => f.DestinationAgency).Index(4);
        Map(f => f.DestinationAccountNumber).Index(5);
        Map(f => f.TransactionType).Index(6);
        Map(f => f.TransactionWay).Index(7);
        Map(f => f.Value).Index(8);
    }
}
public enum TransactionType
{
    DOC,
    TED,
    TEF,
};
public enum TransactionWay
{
    Débito = 0,
    Saída = 0,
    Crédito = 1,
    Entrada = 1
}