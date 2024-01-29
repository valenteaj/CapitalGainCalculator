using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;
using FluentAssertions;
using Moq;
using Moq.AutoMock;

namespace CapitalGainCalculator.CalculationEngine.Tests;

public class Ledger_RegisterTransactionShould
{
    private AutoMocker _mocker = new();
    [Fact]
    public void RegisterTransaction_CalledWithTransaction_AddsTransactionToCollection()
    {
        // Arrange
        var transactions = Enumerable.Empty<Transaction>();
        _mocker.GetMock<IStore<Transaction>>().Setup(_ => _.Get()).Returns(transactions);
        var ledger = _mocker.CreateInstance<Ledger>();
        var transactionDate = new DateTimeOffset();
        var mockAsset = new Asset("Test Asset");
        var t = new Transaction(TransactionType.Purchase, mockAsset, transactionDate, 1, 1, 0);

        // Act
        var fn = () => ledger.RegisterTransaction(t);

        // Assert
        fn.Should().NotThrow();
    }

    [Fact]
    public void RegisterTransaction_CalledWithNull_ThrowsArgumentException()
    {
        // Arrange
        var transactions = Enumerable.Empty<Transaction>();
        _mocker.GetMock<IStore<Transaction>>().Setup(_ => _.Add(null)).Throws<ArgumentException>();
        var ledger = _mocker.CreateInstance<Ledger>();
        var mockAsset = new Mock<Asset>();

        // Act
        var fn = () => ledger.RegisterTransaction(null);

        // Assert
        fn.Should().ThrowExactly<ArgumentException>();
    }
}