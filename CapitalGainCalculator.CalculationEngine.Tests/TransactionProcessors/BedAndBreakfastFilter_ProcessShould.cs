using CapitalGainCalculator.CalculationEngine.TransactionProcessors;
using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;
using FluentAssertions;

namespace CapitalGainCalculator.CalculationEngine.Tests.TransactionProcessors
{
    public class BedAndBreakfastProcessor_ProcessShould
    {
        [Fact]
        public void Process_WithEmptyTransactionList_ShouldPassThrough()
        {
            // Arrange
            ITransactionProcessor classUnderTest = new BedAndBreakfastTransactionProcessor();

            var toBeProcessed = Array.Empty<Transaction>();

            // Act
            var result = classUnderTest.Process(toBeProcessed);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Process_WithTransactionsOutsideBreadAndBreakfastWindow_ShouldBeIgnoredByProcessor()
        {
            // Arrange
            ITransactionProcessor classUnderTest = new BedAndBreakfastTransactionProcessor();
            var asset = new Asset("test");

            var toBeProcessed = new Transaction[] 
            {
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-01T00:00:00Z")), 10, 1, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-31T00:00:00Z")), 5, 1, 1),
            };

            var expectedOutput = new Transaction[] 
            {
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-01T00:00:00Z")), 10, 1, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-31T00:00:00Z")), 5, 1, 1),
            };

            // Act
            var result = classUnderTest.Process(toBeProcessed);

            // Assert
            result.Should().BeEquivalentTo(expectedOutput);
        }

        [Fact]
        public void Process_TotalNeutralisationWithAllTransactionsWithin30Days_ShouldResultInPurchaseSplitIntoSamePricesAsDisposals()
        {
            // Arrange
            ITransactionProcessor classUnderTest = new BedAndBreakfastTransactionProcessor();
            var asset = new Asset("test");
            var toBeProcessed = new Transaction[] 
            {
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-01T00:00:00Z")), 1.2m, 1, 1),
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-15T00:00:00Z")), 1.4m, 1, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-30T00:00:00Z")), 2m, 2, 1),
            };

            var expectedOutput = new Transaction[] 
            {
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-01T00:00:00Z")), 1.2m, 1, 1),
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-15T00:00:00Z")), 1.4m, 1, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-30T00:00:00Z")), 1.4m, 1, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-30T00:00:00Z")), 1.2m, 1, 0),
            };

            // Act
            var result = classUnderTest.Process(toBeProcessed);

            // Assert
            result.Sum(t => t.NumberOfShares).Should().Be(0);
            result.Should().BeEquivalentTo(expectedOutput);
        }

        [Fact]
        public void Process_MultipleSetsOfTotalNeutralisationWithTransactionsWithin30Days_ShouldResultInSetsOfPurchasesSplitIntoSamePricesAsDisposals()
        {
            // Arrange
            ITransactionProcessor classUnderTest = new BedAndBreakfastTransactionProcessor();
            var asset = new Asset("test");
            var toBeProcessed = new Transaction[] 
            {
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-01T00:00:00Z")), 1.2m, 1, 1),
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-02T00:00:00Z")), 1.4m, 1, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-03T00:00:00Z")), 2m, 2, 1),
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-02-01T00:00:00Z")), 2.2m, 1, 1),
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-02-02T00:00:00Z")), 2.4m, 1, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-02-03T00:00:00Z")), 3m, 2, 1),
            };

            var expectedOutput = new Transaction[] 
            {
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-01T00:00:00Z")), 1.2m, 1, 1),
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-02T00:00:00Z")), 1.4m, 1, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-03T00:00:00Z")), 1.4m, 1, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-03T00:00:00Z")), 1.2m, 1, 0),
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-02-01T00:00:00Z")), 2.2m, 1, 1),
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-02-02T00:00:00Z")), 2.4m, 1, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-02-03T00:00:00Z")), 2.4m, 1, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-02-03T00:00:00Z")), 2.2m, 1, 0),
            };

            // Act
            var result = classUnderTest.Process(toBeProcessed);

            // Assert
            result.Sum(t => t.NumberOfShares).Should().Be(0);
            result.Should().BeEquivalentTo(expectedOutput);
        }

        [Fact]
        public void Process_PartialNeutralisationWithAllTransactionsWithin30Days_ShouldResultInPurchaseSplitIntoSamePricesAsDisposals()
        {
            // Arrange
            ITransactionProcessor classUnderTest = new BedAndBreakfastTransactionProcessor();
            var asset = new Asset("test");

            var toBeProcessed = new Transaction[] 
            {
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-01T00:00:00Z")), 1, 4, 1),
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-02T00:00:00Z")), 2, 4, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-03T00:00:00Z")), 3, 7, 1),
            };

            var expectedOutput = new Transaction[] 
            {
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-01T00:00:00Z")), 1, 4, 1),
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-02T00:00:00Z")), 2, 4, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-03T00:00:00Z")), 2, 4, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-03T00:00:00Z")), 1, 3, 0),
            };

            // Act
            var result = classUnderTest.Process(toBeProcessed);

            // Assert
            result.Should().BeEquivalentTo(expectedOutput);
        }

        [Fact]
        public void Process_TotalAndOverflowNeutralisationWithAllTransactionsWithin30Days_ShouldResultInPurchaseSplitIntoSamePricesAsDisposalsWithRemainderAtOriginalPrice()
        {
            // Arrange
            ITransactionProcessor classUnderTest = new BedAndBreakfastTransactionProcessor();
            var asset = new Asset("test");

            var toBeProcessed = new Transaction[] 
            {
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-01T00:00:00Z")), 1, 4, 1),
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-02T00:00:00Z")), 2, 4, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-03T00:00:00Z")), 3, 9, 1),
            };

            var expectedOutput = new Transaction[] 
            {
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-01T00:00:00Z")), 1, 4, 1),
                new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-02T00:00:00Z")), 2, 4, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-03T00:00:00Z")), 2, 4, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-03T00:00:00Z")), 1, 4, 0),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-03T00:00:00Z")), 3, 1, 0),
            };

            // Act
            var result = classUnderTest.Process(toBeProcessed);

            // Assert
            result.Should().BeEquivalentTo(expectedOutput);
        }
    }
}