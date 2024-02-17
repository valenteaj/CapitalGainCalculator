namespace CapitalGainCalculator.CalculationEngine.Interfaces
{
    public interface IStore<T>
    {
        public IEnumerable<T> Get();
        public void Add(T item);
    }
}