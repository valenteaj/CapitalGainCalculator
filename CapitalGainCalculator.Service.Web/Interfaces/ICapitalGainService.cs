using CapitalGainCalculator.Service.Web.DTOs;

namespace CapitalGainCalculator.Service.Web.Interfaces;

public interface ICapitalGainService
{
    public decimal GetTotalCapitalGain(TransactionSet transactions);
}
