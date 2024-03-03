namespace CapitalGainCalculator.Common.Models
{
    public class Asset
    {
        public string Name {get;init;}
        public Asset(string name)
        {
            Name = name;
        }
    }
}