using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine.TransactionProcessors
{
    public class BedAndBreakfastTransactionProcessor : ITransactionProcessor
    {
        private static double CutOffDays = 30;
        public IEnumerable<Transaction> Process(IEnumerable<Transaction> preProcessed)
        {
            var processed = new List<Transaction>();
            var preProcessedOrdered = preProcessed.OrderByDescending(t => t.TransactionDate).ToList();
            
            foreach (var currentTransaction in preProcessedOrdered)
            {
                var toInterrogate = GetPriorTransactionsOutsideSection104(preProcessedOrdered, currentTransaction);
                if (toInterrogate.Any())
                {
                    var neutralised = NeutraliseTransactions(toInterrogate);
                    processed.AddRange(ShardPurchase(neutralised, currentTransaction));
                }
                else 
                {
                    processed.Add(currentTransaction);
                }
            }

            return processed.OrderBy(t => t.TransactionDate);
        }

        private IEnumerable<Transaction> GetPriorTransactionsOutsideSection104(IEnumerable<Transaction> transactions, Transaction candidate)
        {
            if (candidate.TransactionType == TransactionType.Purchase)
            {
                return transactions.Where(t => 
                    t.TransactionDate < candidate.TransactionDate.Date && 
                    t.TransactionDate > candidate.TransactionDate.AddDays(CutOffDays * -1));
            }
            return Array.Empty<Transaction>();
        }

        private IEnumerable<Transaction> NeutraliseTransactions(IEnumerable<Transaction> transactions)
        {
            var orderedDisposals = transactions.Where(t => t.TransactionType == TransactionType.Disposal).OrderByDescending(t => t.TransactionDate);
            var orderedPurchases = transactions.Where(t => t.TransactionType == TransactionType.Purchase).OrderBy(t => t.TransactionDate);
            var residualDisposals = new List<Transaction>(orderedDisposals);

            foreach (var transaction in orderedPurchases)
            {
                var relevantDisposals = residualDisposals
                    .OrderByDescending(t => t.TransactionDate)
                    .Where(t => t.TransactionDate < transaction.TransactionDate);
                var unitCount = transaction.NumberOfShares;
                foreach (var disposal in relevantDisposals)
                {
                    residualDisposals.Remove(disposal);
                    unitCount += disposal.NumberOfShares;
                    if (unitCount < 0)
                    {
                        residualDisposals.Add(new Transaction(
                            TransactionType.Disposal, 
                            transaction.Asset, 
                            transaction.TransactionDate, 
                            transaction.UnitPrice, 
                            unitCount, 
                            transaction.TransactionCosts
                        ));
                        break;
                    }
                }
            }

            return residualDisposals;
        }

        private IEnumerable<Transaction> ShardPurchase(IEnumerable<Transaction> disposals, Transaction purchase)
        {
            var sharded = new List<Transaction>();
            disposals = disposals.OrderByDescending(t => t.TransactionDate);
            var remainingUnits = purchase.NumberOfShares;
            var remainingCosts = purchase.TransactionCosts;

            foreach (var disposal in disposals)
            {
                remainingUnits += disposal.NumberOfShares;
                var units = remainingUnits >= 0 ? disposal.NumberOfShares : disposal.NumberOfShares - remainingUnits;

                sharded.Add(new Transaction(
                    TransactionType.Purchase, 
                    purchase.Asset, 
                    purchase.TransactionDate, 
                    disposal.UnitPrice, 
                    units, 
                    remainingCosts
                ));
                remainingCosts = 0;
            }

            if (remainingUnits > 0)
            {
                sharded.Add(new Transaction(
                    TransactionType.Purchase, 
                    purchase.Asset, 
                    purchase.TransactionDate, 
                    purchase.UnitPrice, 
                    remainingUnits, 
                    remainingCosts
                ));
            }
            return sharded;
        }
    }
}