using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;
using FluentAssertions;
using Moq;
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
            var mockPurchase = new Mock<Purchase>(mockAsset, transactionDate, 1m, 1m, 0m);
            var mockDisposal = new Mock<Disposal>(mockAsset, transactionDate, 1m, 1m, 0m);
            var transactions = new Transaction[] { mockPurchase.Object, mockDisposal.Object };
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
            var transactions = Enumerable.Empty<Transaction>();
            _mocker.GetMock<IStore<Transaction>>().Setup(_ => _.Get()).Returns(transactions);
            var ledger = _mocker.CreateInstance<Ledger>();
            var mockAsset = new Asset("Test Asset");
            var aDifferentAsset = new Asset("A Different Asset");
            var transactionDate = new DateTimeOffset();
            var mockPurchase = new Mock<Purchase>(mockAsset, transactionDate, 1m, 1m, 0m);
            var mockDisposal = new Mock<Disposal>(mockAsset, transactionDate, 1m, 1m, 0m);

            ledger.RegisterTransaction(mockPurchase.Object);
            ledger.RegisterTransaction(mockDisposal.Object);

            // Act
            var result = ledger.GetTransactionsByAsset(aDifferentAsset);

            // Assert
            result.Should().BeEmpty();
        }
    }
}