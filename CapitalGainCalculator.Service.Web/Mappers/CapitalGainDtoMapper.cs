using System.Diagnostics;
using DTO = CapitalGainCalculator.Service.Web.DTOs;
using Engine = CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.Service.Web.Mappers;

public static class CapitalGainDtoMapper
{
    public static IEnumerable<Engine::Transaction> Map(DTO::TransactionSet transaction) 
    {
        var asset = new Engine::Asset(transaction.Asset);
        foreach (var t in transaction.Transactions)
        {
            yield return new Engine::Transaction(
                Map(t.TransactionType),
                asset,
                t.TransactionDate,
                t.UnitPrice,
                t.NumberOfShares,
                t.TransactionCosts
            );
        }
    }
        
    private static Engine::TransactionType Map(DTO::TransactionType transactionType) => transactionType switch
    {
        DTO.TransactionType.Purchase => Engine.TransactionType.Purchase,
        DTO.TransactionType.Disposal => Engine.TransactionType.Disposal,
        _ => throw new UnreachableException($"{nameof(transactionType)} not mapped")
    };
}
