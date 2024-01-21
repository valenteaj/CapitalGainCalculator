using System.Text;
using CapitalGainCalculator.Common.Interfaces;
using CapitalGainCalculator.Common.Models;

namespace CapitalGainCalculator.Common
{
    public class AssetManager : IAssetManager
    {
        private readonly ILedger _ledger;
        public AssetManager(ILedger ledger)
        {
            _ledger = ledger;
        }

        public Purchase Buy(IAsset asset, decimal quantity, decimal unitPrice, decimal transactionCosts, DateTimeOffset? transactionDate = null)
        {
            var transaction = new Purchase(asset, transactionDate ?? DateTimeOffset.UtcNow, unitPrice, quantity, transactionCosts);
            _ledger.RegisterTransaction(transaction);
            return transaction;
        }

        public Disposal Sell(IAsset asset, decimal quantity, decimal currentPrice, decimal transactionCosts, DateTimeOffset? transactionDate = null)
        {
            var transaction = new Disposal(asset, transactionDate ?? DateTimeOffset.UtcNow, currentPrice, quantity, transactionCosts);
            _ledger.RegisterTransaction(transaction);
            return transaction;
        }

        public decimal CalculateChargeableGain(Disposal disposal)
        {
            var disposalProceeds = disposal.NumberOfShares * disposal.UnitPrice;
            var totalNumberOfShares = _ledger.TotalNumberOfShares(disposal.Asset, disposal.TransactionDate);
            var allowableCost = _ledger.TotalProofOfActualCost(disposal.Asset, disposal.TransactionDate) * disposal.NumberOfShares / totalNumberOfShares;
            
            var chargeableGain = ((disposalProceeds - allowableCost) * -1) - disposal.TransactionCosts;
            return chargeableGain;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var assets = _ledger.Assets;
            decimal cumulativeChargeableGain = 0;
            decimal totalPurchases = 0;
            decimal totalPurchasesValue = 0;
            decimal totalDisposals = 0;
            decimal totalDisposalsValue = 0;
            decimal totalFeesPaid = 0;
            foreach (var asset in assets)
            {
                builder.AppendLine(asset.Name);
                builder.AppendLine(string.Empty.PadRight(asset.Name.Length, '='));
                var assetTransactions = _ledger.GetTransactionsByAsset(asset);
                foreach (var transaction in assetTransactions)
                {
                    builder.AppendLine(transaction.ToString());
                    if (transaction is Disposal)
                    {
                        totalDisposalsValue += transaction.NumberOfShares * transaction.UnitPrice;
                        var chargeableGain = CalculateChargeableGain((Disposal)transaction);
                        cumulativeChargeableGain += chargeableGain;
                        ++totalDisposals;
                        builder.AppendLine($"\tChargeable gain: {chargeableGain:C2}");
                    }
                    else 
                    {
                        totalPurchasesValue += transaction.NumberOfShares * transaction.UnitPrice;
                        ++totalPurchases;
                    }
                    totalFeesPaid += transaction.TransactionCosts;
                }
                builder.AppendLine();
            }
            builder.AppendLine("Summary");
            builder.AppendLine("=======");
            builder.AppendLine($"Total Chargeable Gain: {cumulativeChargeableGain:C2}");
            builder.AppendLine($"Total Purchases: {totalPurchasesValue:C2} ({totalPurchases} transactions)");
            builder.AppendLine($"Total Disposals: {totalDisposalsValue:C2} ({totalDisposals} transactions)");
            builder.AppendLine($"Total Fees Paid: {totalFeesPaid:C2}");
            return builder.ToString();
        }
    }
}