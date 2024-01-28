using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;
using FluentAssertions;
using Moq;

namespace CapitalGainCalculator.CalculationEngine.Tests
{
    //public class Ledger_GetTransactionsByAsset
    //{
    //    [Fact]
    //    public void GetCumulativeGainData_WhenNoTransactions_ReturnEmptyEnumerable()
    //    {
    //        // Arrange
    //        ILedger ledger = new Ledger();
    //        var mockAsset = new Mock<Asset>();

    //        // Act
    //        var result = ledger.GetTransactionsByAsset(mockAsset.Object);

    //        // Assert
    //        result.Should().BeEmpty();
    //    }

    //    [Fact]
    //    public void GetCumulativeGainData_WithTwoTransactionsOfTheSameAssetAndQueriedFor_ReturnsThoseTransactions()
    //    {
    //        // Arrange
    //        ILedger ledger = new Ledger();
    //        var mockAsset = new Mock<Asset>();
    //        var transactionDate = new DateTimeOffset();
    //        var mockPurchase = new Mock<Purchase>(mockAsset.Object, transactionDate, 1m, 1m, 0m);
    //        var mockDisposal = new Mock<Disposal>(mockAsset.Object, transactionDate, 1m, 1m, 0m);

    //        ledger.RegisterTransaction(mockPurchase.Object);
    //        ledger.RegisterTransaction(mockDisposal.Object);

    //        // Act
    //        var result = ledger.GetTransactionsByAsset(mockAsset.Object);

    //        // Assert
    //        result.Should().HaveCount(2);
    //    }

    //    [Fact]
    //    public void GetCumulativeGainData_WithTwoTransactionsAndQueriedForADifferentAsset_ReturnsEmptyEnumerable()
    //    {
    //        // Arrange
    //        ILedger ledger = new Ledger();
    //        var mockAsset = new Mock<Asset>();
    //        var aDifferentAsset = new Mock<Asset>();
    //        var transactionDate = new DateTimeOffset();
    //        var mockPurchase = new Mock<Purchase>(mockAsset.Object, transactionDate, 1m, 1m, 0m);
    //        var mockDisposal = new Mock<Disposal>(mockAsset.Object, transactionDate, 1m, 1m, 0m);
            
    //        mockAsset.Setup(a => a.Name).Returns("An asset");
    //        aDifferentAsset.Setup(a => a.Name).Returns("A totally different asset");

    //        ledger.RegisterTransaction(mockPurchase.Object);
    //        ledger.RegisterTransaction(mockDisposal.Object);

    //        // Act
    //        var result = ledger.GetTransactionsByAsset(aDifferentAsset.Object);

    //        // Assert
    //        result.Should().BeEmpty();
    //    }
    //}
}