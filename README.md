# HMRC UK Shares Capital Gains Calculator

## Description

As a UK taxpayer and a holder of shares outside of a tax efficient wrapper (Pension, ISA, etc), I have an obligation to complete a self assessment form each year detailing the amount owed as capital gains tax for any shares in a Section 104 holding disposed of during the financial year outside of my CGT allowance. This is absolute pain to work out given the complex calculation guidelines provided by HMRC and the lack of resources/tools on the web to assist in this labourious task.

My motivation for building this project was to take all of the manual work out of calculating the rolling average purchase price of shares and its subsequent effect on any capital gain therefore making it easy to work out any tax owed.

This currently lives as a library with a backing console app that can be modified to initialise an enumerable of transaction records representing your portfolio then passed into the processor outputting the total gain as a number to the console. Some day, I might get around to turning this into a web API and create a web frontend... maybe.

## Installation

### Requirements
Microsoft .NET 7.0 SDK or above

### Compilation
From the command line, navigate to the root directory of the repository and run the following:
```shell
valenteaj@Latitude-E5450:~/source/CapitalGainCalculator$ dotnet build -c Release
```

### Running Application
```shell
valenteaj@Latitude-E5450:~/source/CapitalGainCalculator$ ./CapitalGainCalculator.ConsoleRunner/bin/Release/net7.0/CapitalGainCalculator.ConsoleRunner
```

## Usage

Some examples have already been added to `CapitalGainCalculator.ConsoleRunner/Program.cs`, but let's go through the example provided by HMRC in the [HS284 Shares and Capital Gains Tax](https://assets.publishing.service.gov.uk/media/5e848d7fe90e0706f5454ffe/HS284_Example_3_2020.pdf) documentation.

The transactions can be summarised as follows:
1. Share Purchase: 1000 x £4.00 + £150 dealing costs
1. Share Purchase: 500 x £4.10 + £80 dealing costs
1. Share Sale: 700 x £4.80 + £100 dealing costs
1. Share Sale: 400 x £5.20 + £105 dealing costs

Total capital gains: £629

### Getting an Overall Summary

Using `AssetManager.ToString()`

```c#
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

var asset1 = new Asset("HMRC HS284 Example 3");
var transactions1 = new List<Transaction>
{
    new Transaction(TransactionType.Purchase, asset1, new DateTimeOffset(new DateTime(2014, 4, 1)), 1000, 4m, 150),
    new Transaction(TransactionType.Purchase, asset1, new DateTimeOffset(new DateTime(2017, 9, 1)), 500, 4.1m, 80),
    new Transaction(TransactionType.Disposal, asset1, new DateTimeOffset(new DateTime(2022, 5, 1)), 700, 4.8m, 100),
    new Transaction(TransactionType.Disposal, asset1, new DateTimeOffset(new DateTime(2023, 2, 1)), 400, 5.2m, 105),
};

try
{
    var processor = new TransactionRuleProcessor(ruleStrategies, mutatorStrategies, new PortfolioValidator());
    var gainData = processor.Process(transactions1);
    Console.WriteLine($"{string.Join('+', gainData.Gains)}={gainData.Gains.Sum():C2}");
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}
```

Output:
```
329+300=£629.00
```

## License

Refer to LICENSE file

## Features
- Aggregation of same-day purchases and disposals [TCGA92/S105 (1)(a)]
- Disposal matching rules
	- Same Day [TCGA92/S105 (1)(b)]
	- Thirty Day [TCGA92/S106A (5)]
	- Section 104 Holding
- Factors in fees for accurate allowance deduction

## Upcoming Development
- Support for multiple assets
- Comprehensive calculation output
- Stability improvements

## Tests

### Unit Tests
Unit tests written using the XUnit testing framework, supplemented by Moq and FluentAssertions extensions.

Tests can be run from the command line under the solution directory as follows:
```shell
valenteaj@Latitude-E5450:~/source/CapitalGainCalculator$ dotnet test
```