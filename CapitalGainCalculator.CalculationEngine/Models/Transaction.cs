namespace CapitalGainCalculator.CalculationEngine.Models;

public record Transaction(
    Asset Asset,
    DateTimeOffset TransactionDate,
    decimal UnitPrice,
    decimal NumberOfShares,
    decimal TransactionCosts,
    TransactionType TransactionType)
{
    public override string ToString()
    {
        return $"{TransactionDate} {TransactionType} [{Asset.Name}]: {NumberOfShares}@{UnitPrice:C2} = {NumberOfShares*UnitPrice:C2}";
    }
}
