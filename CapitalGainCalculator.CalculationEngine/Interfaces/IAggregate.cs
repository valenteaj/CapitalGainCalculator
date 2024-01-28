namespace CapitalGainCalculator.CalculationEngine.Interfaces
{
    public interface IAggregate<T>
    {
        public T Aggregate(T accumulator);
    }
}