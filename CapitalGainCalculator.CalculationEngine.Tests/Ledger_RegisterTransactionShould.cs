using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;
using FluentAssertions;
using Moq;

namespace CapitalGainCalculator.CalculationEngine.Tests;

//public class Ledger_RegisterTransactionShould
//{
//    [Fact]
//    public void RegisterTransaction_CalledWithTransaction_AddsTransactionToCollection()
//    {
//        // Arrange
//        ILedger ledger = new Ledger();
//        var transactionDate = new DateTimeOffset();
//        var mockAsset = new Mock<Asset>();
//        Transaction t = new Purchase(mockAsset.Object, transactionDate, 1, 1, 0);

//        // Act
//        var fn = () => ledger.RegisterTransaction(t);

//        // Assert
//        fn.Should().NotThrow();
//    }

//    [Fact]
//    public void RegisterTransaction_CalledWithNull_ThrowsArgumentException()
//    {
//        // Arrange
//        ILedger ledger = new Ledger();
//        var mockAsset = new Mock<Asset>();

//        // Act
//        var fn = () => ledger.RegisterTransaction(null);

//        // Assert
//        fn.Should().ThrowExactly<ArgumentException>();
//    }
//}