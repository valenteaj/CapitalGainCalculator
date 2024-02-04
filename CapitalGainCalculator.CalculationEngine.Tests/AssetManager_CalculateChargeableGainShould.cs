using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;
using FluentAssertions;
using Moq;

namespace CapitalGainCalculator.CalculationEngine.Tests
{
    public class AssetManager_CalculateChargeableGainShould
    {
        #region Transaction Overload

        [Fact]
        public void CalculateChargeableGain_WhenParameterIsNotDisposal_ShouldThrowError()
        {
            // Arrange
            var asset = new Asset("test-asset");
            var mockLedger = Mock.Of<ILedger>();
            var classUnderTest = new AssetManager(mockLedger);

            var transaction = new Transaction(TransactionType.Purchase, asset, DateTimeOffset.MinValue, 0, 0, 0);

            // Act
            var fn = () => classUnderTest.CalculateChargeableGain(transaction);

            // Assert
            fn.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void CalculateChargeableGain_WhenDisposalIsAppliedAgainstPortfolioWithNoShares_ShouldThrowInvalidDataException()
        {
            // Arrange
            var asset = new Asset("test-asset");
            var timestamp = DateTimeOffset.Parse("2021-01-01T00:00:00Z");
            var mockLedger = new Mock<ILedger>();
            var mockGainData = new CumulativeGainData { TotalNumberOfShares = 0, TotalPoolOfActualCost = 0 };
            mockLedger.Setup(_ => _.GetCumulativeGainData(asset, timestamp)).Returns(mockGainData);
            var classUnderTest = new AssetManager(mockLedger.Object);

            var transaction = new Transaction(TransactionType.Disposal, asset, timestamp, 10m, -1m, 0);

            // Act
            var fn = () => classUnderTest.CalculateChargeableGain(transaction);

            // Assert
            fn.Should().ThrowExactly<InvalidDataException>();
            mockLedger.VerifyAll();
        }

        [Theory]
        [InlineData(100, 1000, -1, 10, 0)]
        [InlineData(100, 1000, -1, 20, 10)]
        [InlineData(100, 1000, -10, 20, 100)]
        [InlineData(100, 1000, -1, 5, -5)]
        public void CalculateChargeableGain_WhenProvidedValidPriceData_ShouldReturnCalculatedGain(
            decimal totalShares, 
            decimal totalPoolOfCost, 
            decimal numberOfSharesSold,
            decimal unitPriceAtSale,
            decimal expectedGain)
        {
            // Arrange
            var asset = new Asset("test-asset");
            var timestamp = DateTimeOffset.Parse("2021-01-01T00:00:00Z");
            var mockLedger = new Mock<ILedger>();
            var mockGainData = new CumulativeGainData 
            { 
                TotalNumberOfShares = totalShares, 
                TotalPoolOfActualCost = totalPoolOfCost 
            };
            mockLedger.Setup(_ => _.GetCumulativeGainData(asset, timestamp)).Returns(mockGainData);
            var classUnderTest = new AssetManager(mockLedger.Object);

            var transaction = new Transaction(TransactionType.Disposal, asset, timestamp, unitPriceAtSale, numberOfSharesSold, 0);

            // Act
            var result = classUnderTest.CalculateChargeableGain(transaction);

            // Assert
            result.Should().Be(expectedGain);
            mockLedger.VerifyAll();
        }

        #endregion
    
        #region Asset Overload
        
        [Fact]
        public void CalculateChargeableGain_WhenLedgerContainsNoDisposals_ShouldNotAttemptToCalcualateGain()
        {
            // Arrange
            var asset = new Asset("test-asset");
            var timestamp = DateTimeOffset.Parse("2021-01-01T00:00:00Z");
            var mockLedger = new Mock<ILedger>(MockBehavior.Strict);

            var testTransactions = new[] 
            {
                new Transaction(TransactionType.Purchase, asset, timestamp, 0, 0, 0),
                new Transaction(TransactionType.Purchase, asset, timestamp, 0, 0, 0),
            };
            
            mockLedger.Setup(_ => _.GetTransactionsByAsset(asset)).Returns(testTransactions);
            var classUnderTest = new AssetManager(mockLedger.Object);

            // Act
            var result = classUnderTest.CalculateChargeableGain(asset);

            // Assert
            result.Should().Be(0);
            mockLedger.VerifyAll();
        }

        [Fact]
        public void CalculateChargeableGain_WhenLedgerContainsDisposal_ShouldCalcualateGain()
        {
            // Arrange
            var asset = new Asset("test-asset");
            var timestamp = DateTimeOffset.Parse("2021-01-01T00:00:00Z");
            var mockLedger = new Mock<ILedger>(MockBehavior.Strict);

            var testTransactions = new[] 
            {
                new Transaction(TransactionType.Purchase, asset, timestamp, 1m, 1m, 0),
                new Transaction(TransactionType.Disposal, asset, timestamp, 2m, 1m, 0),
            };
            
            mockLedger.Setup(_ => _.GetCumulativeGainData(asset, timestamp))
                .Returns(new CumulativeGainData
                {
                    TotalNumberOfShares = 1m,
                    TotalPoolOfActualCost = 1m
                });
            mockLedger.Setup(_ => _.GetTransactionsByAsset(asset)).Returns(testTransactions);
            var classUnderTest = new AssetManager(mockLedger.Object);

            // Act
            var result = classUnderTest.CalculateChargeableGain(asset);

            // Assert
            result.Should().Be(1);
            mockLedger.VerifyAll();
        }

        #endregion

        #region Parameterless Overload
        
        [Fact]
        public void CalculateChargeableGain_WhenLedgerContainsNoDisposals_ShouldNotAttemptToCalcualateGainForAnyAsset()
        {
            // Arrange
            var asset1 = new Asset("test-asset-1");
            var asset2 = new Asset("test-asset-2");
            var timestamp = DateTimeOffset.Parse("2021-01-01T00:00:00Z");
            var mockLedger = new Mock<ILedger>(MockBehavior.Strict);

            var testTransactionsForAsset1 = new[] 
            {
                new Transaction(TransactionType.Purchase, asset1, timestamp, 0, 0, 0),
                new Transaction(TransactionType.Purchase, asset1, timestamp, 0, 0, 0),
            };

            var testTransactionsForAsset2 = new[] 
            {
                new Transaction(TransactionType.Purchase, asset2, timestamp, 0, 0, 0),
                new Transaction(TransactionType.Purchase, asset2, timestamp, 0, 0, 0),
            };
            
            mockLedger.Setup(_ => _.GetTransactionsByAsset(asset1)).Returns(testTransactionsForAsset1);
            mockLedger.Setup(_ => _.GetTransactionsByAsset(asset2)).Returns(testTransactionsForAsset2);
            mockLedger.SetupGet(_ => _.Assets).Returns(new[]{asset1, asset2});
            var classUnderTest = new AssetManager(mockLedger.Object);

            // Act
            var result = classUnderTest.CalculateChargeableGain();

            // Assert
            result.Should().Be(0);
            mockLedger.VerifyAll();
        }

        [Fact]
        public void CalculateChargeableGain_WhenLedgerContainsDisposal_ShouldCalcualateGainForAllAssets()
        {
            // Arrange
            var asset1 = new Asset("test-asset-1");
            var asset2 = new Asset("test-asset-2");
            var timestamp = DateTimeOffset.Parse("2021-01-01T00:00:00Z");
            var mockLedger = new Mock<ILedger>(MockBehavior.Strict);

            var testTransactionsForAsset1 = new[] 
            {
                new Transaction(TransactionType.Purchase, asset1, timestamp, 1m, 1m, 0),
                new Transaction(TransactionType.Disposal, asset1, timestamp, 2m, 1m, 0),
            };

            var testTransactionsForAsset2 = new[] 
            {
                new Transaction(TransactionType.Purchase, asset2, timestamp, 1m, 1m, 0),
                new Transaction(TransactionType.Disposal, asset2, timestamp, 2m, 1m, 0),
            };
            
            mockLedger.Setup(_ => _.GetCumulativeGainData(asset1, timestamp))
                .Returns(new CumulativeGainData
                {
                    TotalNumberOfShares = 1m,
                    TotalPoolOfActualCost = 1m
                });

            mockLedger.Setup(_ => _.GetCumulativeGainData(asset2, timestamp))
                .Returns(new CumulativeGainData
                {
                    TotalNumberOfShares = 1m,
                    TotalPoolOfActualCost = 1m
                });
                        
            mockLedger.Setup(_ => _.GetTransactionsByAsset(asset1)).Returns(testTransactionsForAsset1);
            mockLedger.Setup(_ => _.GetTransactionsByAsset(asset2)).Returns(testTransactionsForAsset2);
            mockLedger.SetupGet(_ => _.Assets).Returns(new[]{asset1, asset2});
            var classUnderTest = new AssetManager(mockLedger.Object);

            // Act
            var result = classUnderTest.CalculateChargeableGain();

            // Assert
            result.Should().Be(2);
            mockLedger.VerifyAll();
        }

        #endregion
    }
}