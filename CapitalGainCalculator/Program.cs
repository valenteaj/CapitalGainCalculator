using CapitalGainCalculator.Common;
using CapitalGainCalculator.Common.Interfaces;
using CapitalGainCalculator.Common.Models;

ILedger ledger = new Ledger();
IAssetManager portfolio = new AssetManager(ledger);

var asset = new Asset("A test asset");
portfolio.Buy(asset, 10, 60m, 3+3, new DateTimeOffset(new DateTime(2024, 1, 10)));
portfolio.Buy(asset, 30, 80m, 3+12, new DateTimeOffset(new DateTime(2024, 1, 11)));
portfolio.Sell(asset, 10, 100m, 3, new DateTimeOffset(new DateTime(2024, 1, 12)));
portfolio.Buy(asset, 10, 120m, 3+6, new DateTimeOffset(new DateTime(2024, 1, 13)));
portfolio.Sell(asset, 10, 150m, 3, new DateTimeOffset(new DateTime(2024, 1, 14)));

var asset2 = new Asset("HMRC HS284 Example 3");
portfolio.Buy(asset2, 1000, 4m, 150, new DateTimeOffset(new DateTime(2014, 4, 1)));
portfolio.Buy(asset2, 500, 4.10m, 80, new DateTimeOffset(new DateTime(2017, 9, 1)));
portfolio.Sell(asset2, 700, 4.8m, 100, new DateTimeOffset(new DateTime(2022, 5, 1)));
portfolio.Sell(asset2, 400, 5.2m, 105, new DateTimeOffset(new DateTime(2023, 2, 1)));

var asset3 = new Asset("Another test asset");
portfolio.Buy(asset3, 0.64421416m, 155.2278826m, 0, new DateTimeOffset(new DateTime(2024, 1, 15)));
portfolio.Sell(asset3, 0.64421416m, 1933.56m, 3, new DateTimeOffset(new DateTime(2024, 1, 16)));

Console.WriteLine(portfolio);