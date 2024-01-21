using CapitalGainCalculator.Common;
using CapitalGainCalculator.Common.Models;

var ledger = new Ledger();
var portfolio = new AssetManager(ledger);

var asset = new Asset("Vanguard FTSE Global All Cap");
portfolio.Buy(asset, 10, 60, 3+3);
portfolio.Buy(asset, 30, 80, 3+12);
Console.WriteLine("Before sell 1 " + ledger.TotalProofOfActualCost(asset).ToString("C2"));
var capGain1 = portfolio.Sell(asset, 10, 100, 3);
Console.WriteLine($"Disposal 1 resulted in gain of {portfolio.CalculateChargeableGain(capGain1):C2}");
Console.WriteLine("After sell 1 " + ledger.TotalProofOfActualCost(asset).ToString("C2"));
portfolio.Buy(asset, 10, 120, 3+6);
Console.WriteLine("Before sell 2 " + ledger.TotalProofOfActualCost(asset).ToString("C2"));
var capGain2 = portfolio.Sell(asset, 10, 150, 3);
Console.WriteLine($"Disposal 2 resulted in gain of {portfolio.CalculateChargeableGain(capGain2):C2}");
Console.WriteLine("After sell 2 " + ledger.TotalProofOfActualCost(asset).ToString("C2"));

var asset2 = new Asset("Vanguard Something of other");
portfolio.Buy(asset2, 1000, 4, 150, new DateTimeOffset(new DateTime(2014, 4, 1)));
Console.WriteLine("After buy 1 " + ledger.TotalProofOfActualCost(asset2).ToString("C2"));
portfolio.Buy(asset2, 500, 4.10m, 80, new DateTimeOffset(new DateTime(2017, 9, 1)));
Console.WriteLine("After buy 2 " + ledger.TotalProofOfActualCost(asset2).ToString("C2"));
var gain1 = portfolio.Sell(asset2, 700, 4.8m, 100, new DateTimeOffset(new DateTime(2022, 5, 1)));
Console.WriteLine($"Disposal 1 resulted in gain of {portfolio.CalculateChargeableGain(gain1):C2}");
Console.WriteLine("After sell 1 " + ledger.TotalProofOfActualCost(asset2).ToString("C2"));
var gain2 = portfolio.Sell(asset2, 400, 5.2m, 105, new DateTimeOffset(new DateTime(2023, 2, 1)));
Console.WriteLine($"Disposal 2 resulted in gain of {portfolio.CalculateChargeableGain(gain2):C2}");
Console.WriteLine("After sell 2 " + ledger.TotalProofOfActualCost(asset2).ToString("C2"));

var asset3 = new Asset("ETH");
portfolio.Buy(asset3, 0.64421416m, 155.2278826m, 0);
Console.WriteLine("Before sell 1 " + ledger.TotalProofOfActualCost(asset3).ToString("C2"));
var capGain4 = portfolio.Sell(asset3, 0.64421416m, 1933.56m, 3);
Console.WriteLine($"Disposal 1 resulted in gain of {portfolio.CalculateChargeableGain(capGain4):C2}");
Console.WriteLine("After sell 1 " + ledger.TotalProofOfActualCost(asset3).ToString("C2"));

Console.WriteLine(ledger);