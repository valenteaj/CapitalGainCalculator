using CapitalGainCalculator.CalculationEngine.Interfaces;
using Engine = CapitalGainCalculator.CalculationEngine.Models;
using CapitalGainCalculator.Service.Web.DTOs;
using CapitalGainCalculator.Service.Web.Interfaces;
using CapitalGainCalculator.Service.Web.Mappers;

namespace CapitalGainCalculator.Service.Web.Services
{
    public class CapitalGainService : ICapitalGainService
    {
        private IAssetManager _assetManager;

        public CapitalGainService(IAssetManager assetManager) 
            => _assetManager = assetManager;

        public decimal GetTotalCapitalGain(TransactionSet transaction)
        {
            var mappedTransactions = CapitalGainDtoMapper.Map(transaction);

            foreach (var t in mappedTransactions)
            {
                if (t.TransactionType == Engine::TransactionType.Purchase)
                {
                    _assetManager.Buy(t.Asset, t.NumberOfShares, t.UnitPrice, t.TransactionCosts, t.TransactionDate);
                }
                else 
                {
                    _assetManager.Sell(t.Asset, t.NumberOfShares, t.UnitPrice, t.TransactionCosts, t.TransactionDate);
                }
            }
            return _assetManager.CalculateChargeableGain();
        }
    }
}