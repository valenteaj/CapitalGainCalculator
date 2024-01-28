using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;
using CapitalGainCalculator.CalculationEngine.Services;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
namespace CapitalGainCalculator.CalculationEngine.Tests
{
    public class Mikes_Ledger_GetCumulativeGainDataShould
    {
        private readonly AutoMocker _mocker = new();

        [Fact]
        public void GetCumulativeGainData_WhenNoTransactions_ReturnsZeroedGainData()
        {
            // Arrange
            var asset = new Asset("Cool Asset");
            var transactions = Enumerable.Empty<Transaction>();

            _mocker.GetMock<ITransactionStore>()
                .Setup(_ => _.Get())
                .Returns(transactions);

            // Act
            ILedger ledger = _mocker.CreateInstance<Ledger>();
            var result = ledger.GetCumulativeGainData(asset);

            // Assert
            result.TotalNumberOfShares.Should().Be(0);
            result.TotalProofOfActualCost.Should().Be(0);
        }

        [Fact]
        public void GetCumulativeGainData_WithTransactions_BuildsAggregate()
        {
            // Arrange
            var asset = new Asset("Cool Asset");
            var transactionDate = new DateTimeOffset();
            var purchase = new Transaction(asset, transactionDate, 1m, 1m, 0m, TransactionType.Purchase);
            var disposal = new Transaction(asset, transactionDate, 1m, 1m, 0m, TransactionType.Disposal);
            var transactions = new Transaction[] { purchase, disposal };

            _mocker.GetMock<ITransactionStore>()
                .Setup(_ => _.Get())
                .Returns(transactions);

            var purchaseServiceResult = new CumulativeGainData()
            {
                TotalNumberOfShares = 1m,
                TotalProofOfActualCost = 300m
            };
            _mocker.GetMock<IPurchaseService>()
                .Setup(_ => _.Aggregate(purchase, It.IsAny<CumulativeGainData>()))
                .Returns(purchaseServiceResult);

            var disposalServiceResult = new CumulativeGainData()
            {
                TotalNumberOfShares = 2m,
                TotalProofOfActualCost = 400m
            };
            _mocker.GetMock<IDisposalService>()
                .Setup(_ => _.Aggregate(disposal, It.IsAny<CumulativeGainData>()))
                .Returns(disposalServiceResult);

            // Act
            ILedger ledger = _mocker.CreateInstance<Ledger>();
            var result = ledger.GetCumulativeGainData(asset);

            // Assert
            result.Should().Be(disposalServiceResult);

            _mocker.VerifyAll();
        }
    }

    //public class Ledger_GetCumulativeGainDataShould
    //{
    //    [Fact]
    //    public void GetCumulativeGainData_WhenNoTransactions_ReturnsZeroedGainData()
    //    {
    //        // Arrange
    //        ILedger ledger = new Ledger();
    //        var mockAsset = new Mock<Asset>();

    //        // Act
    //        var result = ledger.GetCumulativeGainData(mockAsset.Object);

    //        // Assert
    //        result.TotalNumberOfShares.Should().Be(0);
    //        result.TotalProofOfActualCost.Should().Be(0);
    //    }

    //    [Fact]
    //    public void GetCumulativeGainData_WithTransactions_BuildsAggregate()
    //    {
    //        // Arrange
    //        ILedger ledger = new Ledger();
    //        var mockAsset = new Mock<Asset>();
    //        var transactionDate = new DateTimeOffset();
    //        var mockPurchase = new Mock<Purchase>(mockAsset.Object, transactionDate, 1m, 1m, 0m);
    //        mockPurchase.Setup((p) => p.Aggregate(new CumulativeGainData()))
    //            .Returns(new CumulativeGainData 
    //            {
    //                TotalNumberOfShares = 1m,
    //                TotalProofOfActualCost = 300m
    //            });

    //        var mockDisposal = new Mock<Disposal>(mockAsset.Object, transactionDate, 1m, 1m, 0m);
    //        mockDisposal.Setup((p) => p.Aggregate(new CumulativeGainData
    //            {
    //                TotalNumberOfShares = 1m,
    //                TotalProofOfActualCost = 300m
    //            }))
    //            .Returns(new CumulativeGainData 
    //            {
    //                TotalNumberOfShares = 2m,
    //                TotalProofOfActualCost = 400m
    //            });

    //        ledger.RegisterTransaction(mockPurchase.Object);
    //        ledger.RegisterTransaction(mockDisposal.Object);

    //        // Act
    //        var result = ledger.GetCumulativeGainData(mockAsset.Object);

    //        // Assert
    //        result.Should().Be(new CumulativeGainData
    //        {
    //            TotalNumberOfShares = 2m,
    //            TotalProofOfActualCost = 400m
    //        });
    //        mockPurchase.Verify(d => d.Aggregate(It.IsAny<CumulativeGainData>()), Times.Once);
    //        mockDisposal.Verify(d => d.Aggregate(It.IsAny<CumulativeGainData>()), Times.Once);
    //    }
    //}
}