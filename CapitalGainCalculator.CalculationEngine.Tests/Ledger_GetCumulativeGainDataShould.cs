using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;
using FluentAssertions;
using Moq;
using Moq.AutoMock;

namespace CapitalGainCalculator.CalculationEngine.Tests
{
    public class Ledger_GetCumulativeGainDataShould
    {
        private AutoMocker _mocker = new();

        [Fact]
        public void GetCumulativeGainData_WhenNoTransactions_ReturnsZeroedGainData()
        {
            // Arrange
            var transactions = Enumerable.Empty<Transaction>();
            _mocker.GetMock<IStore<Transaction>>().Setup(_ => _.Get()).Returns(transactions);
            var ledger = _mocker.CreateInstance<Ledger>();
            var mockAsset = new Asset("Test Asset");

            // Act
            var result = ledger.GetCumulativeGainData(mockAsset);

            // Assert
            result.TotalNumberOfShares.Should().Be(0);
            result.TotalProofOfActualCost.Should().Be(0);
        }

        [Fact]
        public void GetCumulativeGainData_WithTransactions_BuildsAggregate()
        {
            // Arrange
            var mockAsset = new Asset("Test Asset");
            var transactionDate = new DateTimeOffset();
            var mockPurchase = new Mock<Purchase>(mockAsset, transactionDate, 1m, 1m, 0m);
            mockPurchase.Setup((p) => p.Aggregate(new CumulativeGainData()))
                .Returns(new CumulativeGainData 
                {
                    TotalNumberOfShares = 1m,
                    TotalProofOfActualCost = 300m
                });

            var mockDisposal = new Mock<Disposal>(mockAsset, transactionDate, 1m, 1m, 0m);
            mockDisposal.Setup((p) => p.Aggregate(new CumulativeGainData
                {
                    TotalNumberOfShares = 1m,
                    TotalProofOfActualCost = 300m
                }))
                .Returns(new CumulativeGainData 
                {
                    TotalNumberOfShares = 2m,
                    TotalProofOfActualCost = 400m
                });
            var transactions = new Transaction[] { mockPurchase.Object, mockDisposal.Object };
            _mocker.GetMock<IStore<Transaction>>().Setup(_ => _.Get()).Returns(transactions);
            var ledger = _mocker.CreateInstance<Ledger>();

            // Act
            var result = ledger.GetCumulativeGainData(mockAsset);

            // Assert
            result.Should().Be(new CumulativeGainData
            {
                TotalNumberOfShares = 2m,
                TotalProofOfActualCost = 400m
            });
            mockPurchase.Verify(d => d.Aggregate(It.IsAny<CumulativeGainData>()), Times.Once);
            mockDisposal.Verify(d => d.Aggregate(It.IsAny<CumulativeGainData>()), Times.Once);
        }
    }
}