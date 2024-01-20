using CapitalGainCalculator.Common.Models;

// var ledger = new Ledger();
// var asset = new Asset("Vanguard FTSE Global All Cap", ledger);
// asset.Buy(10, 60, 3+3);
// asset.Buy(30, 80, 3+12);
// Console.WriteLine("Before sell 1 " + ledger.TotalProofOfActualCost.ToString("C2"));
// var capGain1 = asset.Sell(10, 100, 3);
// Console.WriteLine($"Disposal 1 resulted in gain of {capGain1:C2}");
// Console.WriteLine("After sell 1 " + ledger.TotalProofOfActualCost.ToString("C2"));
// asset.Buy(10, 120, 3+6);
// Console.WriteLine("Before sell 2 " + ledger.TotalProofOfActualCost.ToString("C2"));
// var capGain2 = asset.Sell(10, 150, 3);
// Console.WriteLine($"Disposal 2 resulted in gain of {capGain2:C2}");
// Console.WriteLine("After sell 2 " + ledger.TotalProofOfActualCost.ToString("C2"));


// var asset = Asset.CreateObject("ETH");
// asset.Buy(0.64421416m, 155.2278826m, 0);
// Console.WriteLine("Before sell 1 " + asset.TotalProofOfActualCost.ToString("C2"));
// var capGain1 = asset.Sell(0.64421416m, 1933.56m, 3);
// Console.WriteLine($"Disposal 1 resulted in gain of {capGain1:C2}");
// Console.WriteLine("After sell 1 " + asset.TotalProofOfActualCost.ToString("C2"));

var asset2 = Asset.CreateObject("Vanguard Something of other");

asset2.Buy(1000, 4, 150);
Console.WriteLine("After buy 1 " + asset2.TotalProofOfActualCost.ToString("C2"));
asset2.Buy(500, 4.10m, 80);
Console.WriteLine("After buy 2 " + asset2.TotalProofOfActualCost.ToString("C2"));
var gain1 = asset2.Sell(700, 4.8m, 100);
Console.WriteLine($"Disposal 1 resulted in gain of {gain1:C2}");
Console.WriteLine("After sell 1 " + asset2.TotalProofOfActualCost.ToString("C2"));
var gain2 = asset2.Sell(400, 5.2m, 105);
Console.WriteLine($"Disposal 1 resulted in gain of {gain2:C2}");
Console.WriteLine("After sell 2 " + asset2.TotalProofOfActualCost.ToString("C2"));

