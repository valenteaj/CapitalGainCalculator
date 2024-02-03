using CapitalGainCalculator.CalculationEngine;
using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;
using CapitalGainCalculator.CalculationEngine.Strategies;
using CapitalGainCalculator.Service.Web.Interfaces;
using CapitalGainCalculator.Service.Web.Services;

namespace CapitalGainCalculator.Service.Web.Extensions
{
    public static class CapitalGainServiceBuilder
    {
        public static IServiceCollection AddCapitalGainCalcServices(this IServiceCollection services)
        {
            var strategies = new ITransactionStrategy[] 
            {
                new PurchaseStrategy(),
                new DisposalStrategy()
            };
            services.AddScoped<IStore<Transaction>>(_ => new TransactionStore());
            services.AddScoped<ILedger>(_ => new Ledger(
                _.GetService<IStore<Transaction>>()!,
                strategies
            ));
            services.AddScoped<IAssetManager>(_ => new AssetManager(
                _.GetService<ILedger>()!
            ));
            services.AddScoped<ICapitalGainService>(_ => new CapitalGainService(
                _.GetService<IAssetManager>()!
            ));
            return services;
        }
    }
}