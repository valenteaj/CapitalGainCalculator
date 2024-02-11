using CapitalGainCalculator.CalculationEngine.Filters;
using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;
using FluentAssertions;

namespace CapitalGainCalculator.CalculationEngine.Tests.Filters
{
    public class BedAndBreakfastFilter_FilterShould
    {
        [Fact]
        public void Filter_WithEmptyTransactionList_ShouldPassThrough()
        {
            // Arrange
            ITransactionFilter classUnderTest = new BedAndBreakfastFilter();
            var asset = new Asset("test");

            var toBeFiltered = Array.Empty<Transaction>();

            var filterCandidate = new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-03T00:00:00Z")), 1, 2, 1);

            // Act
            var result = classUnderTest.Filter(filterCandidate, toBeFiltered);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Filter_WithTransactionWithin30Days_ShouldHaveQuantityRemoved()
        {
            // Arrange
            ITransactionFilter classUnderTest = new BedAndBreakfastFilter();
            var asset = new Asset("test");

            var toBeFiltered = new Transaction[] 
            {
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2023-01-01T00:00:00Z")), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-01T00:00:00Z")), 1, 1, 1)
            };

            var filterCandidate = new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-02T00:00:00Z")), 1, 1, 1);

            // Act
            var result = classUnderTest.Filter(filterCandidate, toBeFiltered);

            // Assert
            result.Should().HaveCount(2);
            result.Sum(t => t.NumberOfShares).Should().Be(1);
        }

        [Fact]
        public void Filter_TotalDisposalWithAllTransactionsWithin30Days_ShouldHaveAllQuantitiesRemoved()
        {
            // Arrange
            ITransactionFilter classUnderTest = new BedAndBreakfastFilter();
            var asset = new Asset("test");

            var toBeFiltered = new Transaction[] 
            {
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-01T00:00:00Z")), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-02T00:00:00Z")), 1, 1, 1)
            };

            var filterCandidate = new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-03T00:00:00Z")), 1, 2, 1);

            // Act
            var result = classUnderTest.Filter(filterCandidate, toBeFiltered);

            // Assert
            result.Should().HaveCount(2);
            result.Sum(t => t.NumberOfShares).Should().Be(0);
        }

        [Fact]
        public void Filter_PartialDisposalWithAllTransactionWithin30DaysEclipsed_ShouldHaveSomeQuantityRemoved()
        {
            // Arrange
            ITransactionFilter classUnderTest = new BedAndBreakfastFilter();
            var asset = new Asset("test");

            var toBeFiltered = new Transaction[] 
            {
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-01T00:00:00Z")), 1, 5, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-02T00:00:00Z")), 1, 5, 1)
            };

            var filterCandidate = new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-03T00:00:00Z")), 1, 7, 1);

            // Act
            var result = classUnderTest.Filter(filterCandidate, toBeFiltered);

            // Assert
            result.Should().HaveCount(2);
            result.Sum(t => t.NumberOfShares).Should().Be(3);
        }

        [Fact]
        public void Filter_PartialDisposalWithSomeTransactionWithin30DaysEclipsed_ShouldHaveSomeQuantityRemoved()
        {
            // Arrange
            ITransactionFilter classUnderTest = new BedAndBreakfastFilter();
            var asset = new Asset("test");

            var toBeFiltered = new Transaction[] 
            {
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-01T00:00:00Z")), 1, 5, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-02T00:00:00Z")), 1, 5, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-03T00:00:00Z")), 1, 5, 1)
            };

            var filterCandidate = new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-03T00:00:00Z")), 1, 7, 1);

            // Act
            var result = classUnderTest.Filter(filterCandidate, toBeFiltered);

            // Assert
            result.Should().HaveCount(3);
            result.Sum(t => t.NumberOfShares).Should().Be(8);
        }

        [Fact]
        public void Filter_TotalDisposalWithNoTransactionWithin30DaysEclipsed_ShouldRemainUnaffected()
        {
            // Arrange
            ITransactionFilter classUnderTest = new BedAndBreakfastFilter();
            var asset = new Asset("test");

            var toBeFiltered = new Transaction[] 
            {
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2023-01-01T00:00:00Z")), 1, 5, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2023-01-02T00:00:00Z")), 1, 5, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2023-01-03T00:00:00Z")), 1, 5, 1)
            };

            var filterCandidate = new Transaction(TransactionType.Disposal, asset, new DateTimeOffset(DateTime.Parse("2024-01-03T00:00:00Z")), 1, 7, 1);

            // Act
            var result = classUnderTest.Filter(filterCandidate, toBeFiltered);

            // Assert
            result.Should().HaveCount(3);
            result.Sum(t => t.NumberOfShares).Should().Be(15);
        }

        [Fact]
        public void Filter_WithPurchaseCandidate_ShouldNotAffectQuantities()
        {
            // Arrange
            ITransactionFilter classUnderTest = new BedAndBreakfastFilter();
            var asset = new Asset("test");

            var toBeFiltered = new Transaction[] 
            {
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-01T00:00:00Z")), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-02T00:00:00Z")), 1, 1, 1)
            };

            var filterCandidate = new Transaction(TransactionType.Purchase, asset, new DateTimeOffset(DateTime.Parse("2024-01-03T00:00:00Z")), 1, 2, 1);

            // Act
            var result = classUnderTest.Filter(filterCandidate, toBeFiltered);

            // Assert
            result.Should().HaveCount(2);
            result.Sum(t => t.NumberOfShares).Should().Be(2);
        }
    }
}