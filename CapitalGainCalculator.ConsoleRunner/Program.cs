using CapitalGainCalculator.CalculationEngine;
using CapitalGainCalculator.CalculationEngine.Strategies.Rule;
using CapitalGainCalculator.CalculationEngine.Strategies.Mutator;
using CapitalGainCalculator.CalculationEngine.Validators;
using CapitalGainCalculator.Common.Interfaces;
using CapitalGainCalculator.Common.Models;

var ruleStrategies = new IRuleStrategy[]
{
    new ThirtyDayRuleStrategy(),
    new Section104HoldingRuleStrategy(),
    new SameDayRuleStrategy(),
};

var mutatorStrategies = new IMutatorStrategy[]
{
    new SameDayPurchaseMutatorStrategy(),
    new SameDayDisposalMutatorStrategy()
};

var asset = new Asset("test");
var transactions = new List<Transaction>
{
    // new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(new DateTime(2024, 1, 10)), 10, 60m, 0),
    // new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(new DateTime(2024, 1, 11)), 10, 60m, 0),
    // new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(new DateTime(2024, 2, 10)), 15, 120m, 0),
    // new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(new DateTime(2024, 4, 14)), 10, 60m, 0),
    // new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(new DateTime(2024, 4, 15)), 10, 60m, 0),
    // new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(new DateTime(2024, 4, 16)), 10, 60m, 0),
    // new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(new DateTime(2024, 4, 16)), 10, 60m, 0),
    // new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(new DateTime(2024, 4, 16)), 10, 60m, 0),

    new Transaction( TransactionType.Purchase, asset, new DateTimeOffset(new DateTime(2014, 4, 1)), 4m, 1000, 150),
    new Transaction( TransactionType.Purchase, asset, new DateTimeOffset(new DateTime(2017, 4, 1)), 4.1m, 500, 80),
    new Transaction( TransactionType.Disposal, asset, new DateTimeOffset(new DateTime(2022, 4, 1)), 4.8m, 7000, 100),
    new Transaction( TransactionType.Purchase, asset, new DateTimeOffset(new DateTime(2022, 4, 1)), 5.2m, 300, 105),
    new Transaction( TransactionType.Purchase, asset, new DateTimeOffset(new DateTime(2022, 4, 2)), 5.4m, 300, 105),
    new Transaction( TransactionType.Purchase, asset, new DateTimeOffset(new DateTime(2022, 4, 3)), 5.6m, 300, 105),
    new Transaction( TransactionType.Disposal, asset, new DateTimeOffset(new DateTime(2023, 4, 1)), 4.8m, 1700, 100),
};

try
{
    var processor = new TransactionRuleProcessor(ruleStrategies, mutatorStrategies, new PortfolioValidator());
    var gainData = processor.Process(transactions);
    Console.WriteLine(gainData.Gains.Sum());
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}