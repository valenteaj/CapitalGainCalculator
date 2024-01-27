namespace CapitalGainCalculator.Common.Interfaces
{
    public interface IAggregate<T>
    {
        public T Aggregate(T accumulator);
    }
}