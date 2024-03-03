using CapitalGainCalculator.Common.Models;

namespace CapitalGainCalculator.Common.Interfaces
{
    public interface IReporter<T>
    {
        public T GenerateReport(ReportData gainData);
    }
}