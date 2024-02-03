using System.Text;
using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine
{
    public class AssetManager : IAssetManager
    {
        private readonly ILedger _ledger;
        public AssetManager(ILedger ledger)
        {
            _ledger = ledger;
        }

        public Transaction Buy(Asset asset, decimal quantity, decimal unitPrice, decimal transactionCosts, DateTimeOffset? transactionDate = null)
        {
            var transaction = new Transaction(TransactionType.Purchase, asset, transactionDate ?? DateTimeOffset.UtcNow, unitPrice, quantity, transactionCosts);
            _ledger.RegisterTransaction(transaction);
            return transaction;
        }

        public Transaction Sell(Asset asset, decimal quantity, decimal currentPrice, decimal transactionCosts, DateTimeOffset? transactionDate = null)
        {
            var transaction = new Transaction(TransactionType.Disposal, asset, transactionDate ?? DateTimeOffset.UtcNow, currentPrice, -quantity, transactionCosts);
            _ledger.RegisterTransaction(transaction);
            return transaction;
        }

        public decimal CalculateChargeableGain()
        {
            decimal cumulativeGain = 0;
            foreach (var asset in _ledger.Assets)
            {
                cumulativeGain += CalculateChargeableGain(asset);
            }
            return cumulativeGain;
        }

        public decimal CalculateChargeableGain(Asset asset)
        {
            var disposals = _ledger.GetTransactionsByAsset(asset).Where(t => t.TransactionType == TransactionType.Disposal);
            decimal cumulativeGain = 0;
            foreach (var disposal in disposals)
            {
                cumulativeGain += CalculateChargeableGain(disposal);
            }
            return cumulativeGain;
        }

        public decimal CalculateChargeableGain(Transaction disposal)
        {
            if (disposal.TransactionType != TransactionType.Disposal)
                throw new ArgumentException("Transaction must be a disposal", "disposal");
            var disposalProceeds = disposal.NumberOfShares * disposal.UnitPrice;
            var gainData = _ledger.GetCumulativeGainData(disposal.Asset, disposal.TransactionDate);
            if (gainData.TotalNumberOfShares <= 0) 
                throw new InvalidDataException($"No remaining shares to dispose on asset {disposal.Asset.Name}");
            var allowableCost = gainData.TotalProofOfActualCost * disposal.NumberOfShares / gainData.TotalNumberOfShares;
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
                    if (transaction.TransactionType == TransactionType.Disposal)
                    {
                        totalDisposalsValue += transaction.NumberOfShares * transaction.UnitPrice;
                        var chargeableGain = CalculateChargeableGain(transaction);
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