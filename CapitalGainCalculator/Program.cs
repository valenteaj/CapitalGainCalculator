using CapitalGainCalculator.Common;
using CapitalGainCalculator.Common.Interfaces;
using CapitalGainCalculator.Common.Models;

ILedger ledger = new Ledger();
IAssetManager portfolio = new AssetManager(ledger);

var asset = new Asset("Vanguard FTSE Global All Cap");
portfolio.Buy(asset, 10, 60, 3+3, new DateTimeOffset(new DateTime(2024, 1, 10)));
portfolio.Buy(asset, 30, 80, 3+12, new DateTimeOffset(new DateTime(2024, 1, 11)));
portfolio.Sell(asset, 10, 100, 3, new DateTimeOffset(new DateTime(2024, 1, 12)));
portfolio.Buy(asset, 10, 120, 3+6, new DateTimeOffset(new DateTime(2024, 1, 13)));
portfolio.Sell(asset, 10, 150, 3, new DateTimeOffset(new DateTime(2024, 1, 14)));

var asset2 = new Asset("HMRC HS284 Example 3");
portfolio.Buy(asset2, 1000, 4, 150, new DateTimeOffset(new DateTime(2014, 4, 1)));
portfolio.Buy(asset2, 500, 4.10m, 80, new DateTimeOffset(new DateTime(2017, 9, 1)));
portfolio.Sell(asset2, 700, 4.8m, 100, new DateTimeOffset(new DateTime(2022, 5, 1)));
portfolio.Sell(asset2, 400, 5.2m, 105, new DateTimeOffset(new DateTime(2023, 2, 1)));

var asset3 = new Asset("ETH");
portfolio.Buy(asset3, 0.64421416m, 155.2278826m, 0, new DateTimeOffset(new DateTime(2024, 1, 15)));
portfolio.Sell(asset3, 0.64421416m, 1933.56m, 3, new DateTimeOffset(new DateTime(2024, 1, 16)));

Console.WriteLine(portfolio);