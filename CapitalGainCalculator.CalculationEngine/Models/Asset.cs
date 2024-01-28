using CapitalGainCalculator.CalculationEngine.Interfaces;

namespace CapitalGainCalculator.CalculationEngine.Models
{
    public class Asset : IAsset
    {
        public string Name {get;init;}
        public Asset(string name)
        {
            Name = name;
        }
    }
}