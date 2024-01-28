using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;
using FluentAssertions;
using Moq;

namespace CapitalGainCalculator.CalculationEngine.Tests
{
    public class Ledger_GetCumulativeGainDataShould
    {
        [Fact]
        public void GetCumulativeGainData_WhenNoTransactions_ReturnsZeroedGainData()
        {
            // Arrange
            ILedger ledger = new Ledger();
            var mockAsset = new Mock<IAsset>();

            // Act
            var result = ledger.GetCumulativeGainData(mockAsset.Object);

            // Assert
            result.TotalNumberOfShares.Should().Be(0);
            result.TotalProofOfActualCost.Should().Be(0);
        }

        [Fact]
        public void GetCumulativeGainData_WithTransactions_BuildsAggregate()
        {
            // Arrange
            ILedger ledger = new Ledger();
            var mockAsset = new Mock<IAsset>();
            var transactionDate = new DateTimeOffset();
            var mockPurchase = new Mock<Purchase>(mockAsset.Object, transactionDate, 1m, 1m, 0m);
            mockPurchase.Setup((p) => p.Aggregate(new CumulativeGainData()))
                .Returns(new CumulativeGainData 
                {
                    TotalNumberOfShares = 1m,
                    TotalProofOfActualCost = 300m
                });

            var mockDisposal = new Mock<Disposal>(mockAsset.Object, transactionDate, 1m, 1m, 0m);
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

            ledger.RegisterTransaction(mockPurchase.Object);
            ledger.RegisterTransaction(mockDisposal.Object);

            // Act
            var result = ledger.GetCumulativeGainData(mockAsset.Object);

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