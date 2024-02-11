namespace CapitalGainCalculator.CalculationEngine.Interfaces
{
    public interface IFilter<T>
    {
        public IEnumerable<T> Filter(T filterCandidate, IEnumerable<T> toBeFiltered);
    }
}