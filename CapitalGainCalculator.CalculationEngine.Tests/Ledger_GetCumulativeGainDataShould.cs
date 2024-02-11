using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;
using CapitalGainCalculator.CalculationEngine.Strategies;
using FluentAssertions;
using Moq.AutoMock;

namespace CapitalGainCalculator.CalculationEngine.Tests
{
    public class Ledger_GetCumulativeGainDataShould
    {
        private AutoMocker _mocker = new();

        [Fact]
        public void GetCumulativeGainData_WithTransactions_BuildsAggregate()
        {
            // Arrange
            var mockAsset = new Asset("Test Asset");
            var transactionDate = new DateTimeOffset();
            var purchase = new Transaction(TransactionType.Purchase, mockAsset, transactionDate, 1m, 1m, 0m);
            var disposal = new Transaction(TransactionType.Disposal, mockAsset, transactionDate, 1m, -1m, 0m);
            var transactions = new Transaction[] { purchase, disposal };
            var strategies = new List<ITransactionStrategy> { new PurchaseStrategy(), new DisposalStrategy() };
            _mocker.GetMock<IStore<Transaction>>().Setup(_ => _.Get()).Returns(transactions);
            _mocker.Use<IEnumerable<ITransactionStrategy>>(strategies);
            var classUnderTest = _mocker.CreateInstance<Ledger>();

            // Act
            var result = classUnderTest.GetCumulativeGainData(disposal);

            // Assert
            result.Should().Be(new CumulativeGainData
            {
                TotalNumberOfShares = 0m,
                TotalProofOfActualCost = 0m
            });
        }
    }
}