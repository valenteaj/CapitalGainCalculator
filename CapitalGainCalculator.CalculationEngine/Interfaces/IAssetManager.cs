using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine.Interfaces
{
    public interface IAssetManager
    {
        /// <summary>
        /// Registers a purchase <see cref="Transaction"/> into the portfolio
        /// </summary>
        /// <param name="asset">The asset the purchase is attributed to</param>
        /// <param name="quantity">The number of shares of this asset being purchased</param>
        /// <param name="unitPrice">The unit price of the chosen asset at point of purchase</param>
        /// <param name="transactionCosts">Total transaction costs associated with this purchase (Platform, stamp duty, etc)</param>
        /// <param name="transactionDate">The datetime of the transaction (Optional)</param>
        /// <returns>A <see cref="Purchase"/> instance crystalising the transaction</returns>
        public Transaction Buy(Asset asset, decimal quantity, decimal unitPrice, decimal transactionCosts, DateTimeOffset? transactionDate = null);
        
        /// <summary>
        /// Registers a dispsal <see cref="Transaction"/> into the portfolio
        /// </summary>
        /// <param name="asset">The asset the disposal is attributed to</param>
        /// <param name="quantity">The number of shares of this asset being disposed of</param>
        /// <param name="unitPrice">The unit price of the chosen asset at point of disposal</param>
        /// <param name="transactionCosts">Total transaction costs associated with this disposal (Platform, etc)</param>
        /// <param name="transactionDate">The datetime of the transaction (Optional)</param>
        /// <returns>A <see cref="Disposal"/> instance crystalising the transaction</returns>
        public Transaction Sell(Asset asset, decimal quantity, decimal currentPrice, decimal transactionCosts, DateTimeOffset? transactionDate = null);
        
        /// <summary>
        /// Calculate chargeable gain for an entire portfolio
        /// </summary>
        /// <returns>Total chargeable gain expressed as currency</returns>
        public decimal CalculateChargeableGain();

        /// <summary>
        /// Calculate chargeable gain for a specific disposal transaction
        /// </summary>
        /// <param name="disposal">The disposal to calculate gain on</param>
        /// <returns>Chargeable gain expressed as currency</returns>
        public decimal CalculateChargeableGain(Transaction disposal);

        /// <summary>
        /// Calculate chargeable gain for a particular asset
        /// </summary>
        /// <param name="asset">The asset to calculate total gain on</param>
        /// <returns>Total chargeable gain expressed as currency</returns>
        public decimal CalculateChargeableGain(Asset asset);
    }
}