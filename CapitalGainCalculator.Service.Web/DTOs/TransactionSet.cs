namespace CapitalGainCalculator.Service.Web.DTOs
{
    public class TransactionSet
    {
        public string Asset {get; set;}
        public IEnumerable<AssetTransaction> Transactions {get;set;}
    }
}
