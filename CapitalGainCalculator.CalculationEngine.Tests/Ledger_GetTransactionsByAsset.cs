using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;
using CapitalGainCalculator.CalculationEngine.Strategies;
using FluentAssertions;
using Moq.AutoMock;

namespace CapitalGainCalculator.CalculationEngine.Tests
{
    public class Ledger_GetTransactionsByAsset
    {
        private AutoMocker _mocker = new();
        [Fact]
        public void GetCumulativeGainData_WhenNoTransactions_ReturnEmptyEnumerable()
        {
            // Arrange
            var transactions = Enumerable.Empty<Transaction>();
            _mocker.GetMock<IStore<Transaction>>().Setup(_ => _.Get()).Returns(transactions);
            var ledger = _mocker.CreateInstance<Ledger>();
            var mockAsset = new Asset("Test Asset");

            // Act
            var result = ledger.GetTransactionsByAsset(mockAsset);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetCumulativeGainData_WithTwoTransactionsOfTheSameAssetAndQueriedFor_ReturnsThoseTransactions()
        {
            // Arrange
            var mockAsset = new Asset("Test Asset");
            var transactionDate = new DateTimeOffset();
            var purchase = new Transaction(TransactionType.Purchase, mockAsset, transactionDate, 1m, 1m, 0m);
            var disposal = new Transaction(TransactionType.Disposal, mockAsset, transactionDate, 1m, 1m, 0m);
            var transactions = new Transaction[] { purchase, disposal };

            _mocker.GetMock<IStore<Transaction>>().Setup(_ => _.Get()).Returns(transactions);
            var ledger = _mocker.CreateInstance<Ledger>();

            // Act
            var result = ledger.GetTransactionsByAsset(mockAsset);

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public void GetCumulativeGainData_WithTwoTransactionsAndQueriedForADifferentAsset_ReturnsEmptyEnumerable()
        {
            // Arrange
            
            var mockAsset = new Asset("Test Asset");
            var transactionDate = new DateTimeOffset();
            var purchase = new Transaction(TransactionType.Purchase, mockAsset, transactionDate, 1m, 1m, 0m);
            var disposal = new Transaction(TransactionType.Disposal, mockAsset, transactionDate, 1m, 1m, 0m);
            var transactions = new Transaction[] { purchase, disposal };
            _mocker.GetMock<IStore<Transaction>>().Setup(_ => _.Get()).Returns(transactions);
            var ledger = _mocker.CreateInstance<Ledger>();
            var aDifferentAsset = new Asset("A Different Asset");

            // Act
            var result = ledger.GetTransactionsByAsset(aDifferentAsset);

            // Assert
            result.Should().BeEmpty();
        }
    }
}