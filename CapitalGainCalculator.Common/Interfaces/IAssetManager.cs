namespace CapitalGainCalculator.Common.Interfaces
{
    public interface IAssetManager
    {
        public void Buy(IAsset asset, decimal quantity, decimal unitPrice, decimal transactionCosts);
        public decimal Sell(IAsset asset, decimal quantity, decimal currentPrice, decimal transactionCosts);
    }
}