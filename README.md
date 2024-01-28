# HMRC UK Shares Capital Gains Calculator

## Description

As a UK taxpayer and a holder of shares outside of a tax efficient wrapper (Pension, ISA, etc), I have an obligation to complete a self assessment form each year detailing the amount owed as capital gains tax for any shares in a Section 104 holding disposed of during the financial year outside of my CGT allowance. This is absolute pain to work out given the complex calculation guidelines provided by HMRC and the lack of resources/tools on the web to assist in this labourious task.

My motivation for building this project was to take all of the manual work out of calculating the rolling average purchase price of shares and its subsequent effect on any capital gain therefore making it easy to work out any tax owed.

This currently lives as a library with a backing console app that can be modified to initialise an asset manager and ledger; register transactions and then output to the console as a gains summary. Some day, I might get around to turning this into a web API and create a web frontend... maybe.

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
valenteaj@Latitude-E5450:~/source/CapitalGainCalculator$ ./CapitalGainCalculator/bin/Release/net7.0/CapitalGainCalculator 
```

## Usage

Some examples have already been added to `CapitalGainCalculator/Program.cs`, but let's go through the example provided by HMRC in the [HS284 Shares and Capital Gains Tax](https://assets.publishing.service.gov.uk/media/5e848d7fe90e0706f5454ffe/HS284_Example_3_2020.pdf) documentation.

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
using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;

ILedger ledger = new Ledger(); // Ledger keeps a record of transactions for all assets
IAssetManager portfolio = new AssetManager(ledger); // AssetManager facilitates the purchase/sale of a given asset

var asset = new Asset("HMRC HS284 Example 3");
portfolio.Buy(asset, 1000, 4m, 150, new DateTimeOffset(new DateTime(2014, 4, 1)));
portfolio.Buy(asset, 500, 4.10m, 80, new DateTimeOffset(new DateTime(2017, 9, 1)));
portfolio.Sell(asset, 700, 4.8m, 100, new DateTimeOffset(new DateTime(2022, 5, 1)));
portfolio.Sell(asset, 400, 5.2m, 105, new DateTimeOffset(new DateTime(2023, 2, 1)));

Console.WriteLine(portfolio);
```

Output:
```
HMRC HS284 Example 3
====================
01/04/2014 00:00:00 +01:00 Purchase [HMRC HS284 Example 3]: 1000@£4.00 = £4,000.00
01/09/2017 00:00:00 +01:00 Purchase [HMRC HS284 Example 3]: 500@£4.10 = £2,050.00
01/05/2022 00:00:00 +01:00 Disposal [HMRC HS284 Example 3]: -700@£4.80 = -£3,360.00
	Chargeable gain: £329.33
01/02/2023 00:00:00 +00:00 Disposal [HMRC HS284 Example 3]: -400@£5.20 = -£2,080.00
	Chargeable gain: £300.33

Summary
=======
Total Chargeable Gain: £629.67
Total Purchases: £6,050.00 (2 transactions)
Total Disposals: -£5,440.00 (2 transactions)
Total Fees Paid: £435.00
```

### Getting the Gain for a Single Disposal

Using `IAssetManager: decimal CalculateChargeableGain(Disposal disposal)` overload

```c#
using CapitalGainCalculator.CalculationEngine;
using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;

ILedger ledger = new Ledger(); // Ledger keeps a record of transactions for all assets
IAssetManager portfolio = new AssetManager(ledger); // AssetManager facilitates the purchase/sale of a given asset

var asset = new Asset("HMRC HS284 Example 3");
portfolio.Buy(asset, 1000, 4m, 150, new DateTimeOffset(new DateTime(2014, 4, 1)));
portfolio.Buy(asset, 500, 4.10m, 80, new DateTimeOffset(new DateTime(2017, 9, 1)));
var firstDisposal = portfolio.Sell(asset, 700, 4.8m, 100, new DateTimeOffset(new DateTime(2022, 5, 1)));
portfolio.Sell(asset, 400, 5.2m, 105, new DateTimeOffset(new DateTime(2023, 2, 1)));

var chargeableGain = portfolio.CalculateChargeableGain(firstDisposal);
Console.WriteLine(chargeableGain.ToString("C2"));
```

Output:
```
£329.33
```

### Getting the Overall Gain for an Asset

Using `IAssetManager: decimal CalculateChargeableGain(IAsset disposal)` overload

```c#
using CapitalGainCalculator.CalculationEngine;
using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;

ILedger ledger = new Ledger(); // Ledger keeps a record of transactions for all assets
IAssetManager portfolio = new AssetManager(ledger); // AssetManager facilitates the purchase/sale of a given asset

var asset = new Asset("HMRC HS284 Example 3");
portfolio.Buy(asset, 1000, 4m, 150, new DateTimeOffset(new DateTime(2014, 4, 1)));
portfolio.Buy(asset, 500, 4.10m, 80, new DateTimeOffset(new DateTime(2017, 9, 1)));
portfolio.Sell(asset, 700, 4.8m, 100, new DateTimeOffset(new DateTime(2022, 5, 1)));
portfolio.Sell(asset, 400, 5.2m, 105, new DateTimeOffset(new DateTime(2023, 2, 1)));

var chargeableGain = portfolio.CalculateChargeableGain(asset);
Console.WriteLine(chargeableGain.ToString("C2"));
```

Output:
```
£629.67
```

### Getting the Overall Gain for Entire Portfolio

Using `IAssetManager: decimal CalculateChargeableGain()` overload

```c#
using CapitalGainCalculator.CalculationEngine;
using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;

ILedger ledger = new Ledger(); // Ledger keeps a record of transactions for all assets
IAssetManager portfolio = new AssetManager(ledger); // AssetManager facilitates the purchase/sale of a given asset

var asset = new Asset("HMRC HS284 Example 3");
portfolio.Buy(asset, 1000, 4m, 150, new DateTimeOffset(new DateTime(2014, 4, 1)));
portfolio.Buy(asset, 500, 4.10m, 80, new DateTimeOffset(new DateTime(2017, 9, 1)));
portfolio.Sell(asset, 700, 4.8m, 100, new DateTimeOffset(new DateTime(2022, 5, 1)));
portfolio.Sell(asset, 400, 5.2m, 105, new DateTimeOffset(new DateTime(2023, 2, 1)));

var asset2 = new Asset("A Different Asset");
portfolio.Buy(asset2, 10, 60, 3+3, new DateTimeOffset(new DateTime(2024, 1, 10)));
portfolio.Buy(asset2, 30, 80, 3+12, new DateTimeOffset(new DateTime(2024, 1, 11)));
portfolio.Sell(asset2, 10, 100, 3, new DateTimeOffset(new DateTime(2024, 1, 12)));
portfolio.Buy(asset2, 10, 120, 3+6, new DateTimeOffset(new DateTime(2024, 1, 13)));
portfolio.Sell(asset2, 10, 150, 3, new DateTimeOffset(new DateTime(2024, 1, 14)));

var chargeableGain = portfolio.CalculateChargeableGain();
Console.WriteLine(chargeableGain.ToString("C2"));
```

Output:
```
£1,499.73
```

## License

Refer to LICENSE file

## Features
- Support for multiple assets
- Factors in fees for accurate allowance deduction

## Upcoming Development
- Acknowledgement of Bed and Breakfasting rules (Outside a Section 104 holding)
- Stability improvements
- Unit test coverage
- Injection of IStore for ledger

## Tests

TBA
