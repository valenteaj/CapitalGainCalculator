using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapitalGainCalculator.Common.Interfaces
{
    public interface IAsset
    {
        string Name {get; init;}
        decimal TotalProofOfActualCost {get;}
        public void Buy(decimal quantity, decimal unitPrice, decimal transactionCosts);
        public decimal Sell(decimal quantity, decimal currentPrice, decimal transactionCosts);
    }
}