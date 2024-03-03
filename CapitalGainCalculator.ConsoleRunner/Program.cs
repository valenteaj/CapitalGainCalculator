using CapitalGainCalculator.CalculationEngine;
using CapitalGainCalculator.CalculationEngine.Strategies.Rule;
using CapitalGainCalculator.CalculationEngine.Strategies.Mutator;
using CapitalGainCalculator.CalculationEngine.Validators;
using CapitalGainCalculator.Common.Interfaces;
using CapitalGainCalculator.Common.Models;
using CapitalGainCalculator.ReportGenerator.Reporters;

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

var asset1 = new Asset("HMRC HS284 Example 3");
var transactions1 = new List<Transaction>
{
    new Transaction(TransactionType.Purchase, asset1, new DateTimeOffset(new DateTime(2014, 4, 1)), 1000, 4m, 150),
    new Transaction(TransactionType.Purchase, asset1, new DateTimeOffset(new DateTime(2017, 9, 1)), 500, 4.1m, 80),
    new Transaction(TransactionType.Disposal, asset1, new DateTimeOffset(new DateTime(2022, 5, 1)), 700, 4.8m, 100),
    new Transaction(TransactionType.Disposal, asset1, new DateTimeOffset(new DateTime(2023, 2, 1)), 400, 5.2m, 105),
};

var asset2 = new Asset("A test asset");
var transactions2 = new List<Transaction>
{
    new Transaction(TransactionType.Purchase, asset2, new DateTimeOffset(new DateTime(2024, 4, 1)), 10, 60m, 6),
    new Transaction(TransactionType.Purchase, asset2, new DateTimeOffset(new DateTime(2024, 4, 11)), 30, 80m, 15),
    new Transaction(TransactionType.Disposal, asset2, new DateTimeOffset(new DateTime(2024, 4, 12)), 10, 100m, 3),
    new Transaction(TransactionType.Purchase, asset2, new DateTimeOffset(new DateTime(2024, 4, 13)), 10, 120m, 9),
    new Transaction(TransactionType.Disposal, asset2, new DateTimeOffset(new DateTime(2024, 4, 14)), 10, 150m, 3),
};


var asset3 = new Asset("Another test asset");
var transactions3 = new List<Transaction>
{
    new Transaction(TransactionType.Purchase, asset3, new DateTimeOffset(new DateTime(2018, 1, 15)), 0.64421416m, 155.2278826m, 0),
    new Transaction(TransactionType.Disposal, asset3, new DateTimeOffset(new DateTime(2024, 1, 16)), 0.64421416m, 1933.56m, 3),
};

try
{
    var processor = new TransactionRuleProcessor(ruleStrategies, mutatorStrategies, new PortfolioValidator());
    var reporter = new DecimalGainReporter();
    var gainData = processor.Process(transactions1, reporter);
    Console.WriteLine($"{gainData:C2}");

    gainData = processor.Process(transactions2, reporter);
    Console.WriteLine($"{gainData:C2}");

    gainData = processor.Process(transactions3, reporter);
    Console.WriteLine($"{gainData:C2}");
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}