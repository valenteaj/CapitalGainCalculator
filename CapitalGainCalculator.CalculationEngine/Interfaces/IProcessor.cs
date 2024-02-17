namespace CapitalGainCalculator.CalculationEngine.Interfaces
{
    public interface IProcessor<T>
    {
        public IEnumerable<T> Process(IEnumerable<T> preProcessed);
    }
}