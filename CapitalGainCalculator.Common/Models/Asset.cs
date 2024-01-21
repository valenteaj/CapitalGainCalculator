using CapitalGainCalculator.Common.Interfaces;

namespace CapitalGainCalculator.Common.Models
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