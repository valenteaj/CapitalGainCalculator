namespace CapitalGainCalculator.Service.Web.DTOs
{
    public class AssetTransaction
    {
        public decimal UnitPrice {get; set;}
        public decimal NumberOfShares {get; set;}
        public decimal TransactionCosts {get; set;}
        public DateTimeOffset TransactionDate {get; set;}
        public TransactionType TransactionType {get; set;}
    }
}