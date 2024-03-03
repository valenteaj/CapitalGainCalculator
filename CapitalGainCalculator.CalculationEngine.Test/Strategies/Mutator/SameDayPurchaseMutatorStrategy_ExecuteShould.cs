using CapitalGainCalculator.Common.Interfaces;
using CapitalGainCalculator.Common.Models;
using CapitalGainCalculator.CalculationEngine.Strategies.Mutator;
using FluentAssertions;

namespace CapitalGainCalculator.CalculationEngine.Test.Strategies.Mutator
{
    public class SameDayPurchaseMutatorStrategy_ExecuteShould
    {
        [Fact]
        public void Execute_WhenPassedEmptyEnumerable_ShouldReturnEmptyEnumerable()
        {
            IMutatorStrategy classUnderTest = new SameDayPurchaseMutatorStrategy();
            var result = classUnderTest.Execute(Array.Empty<Transaction>());
            result.Should().BeEmpty();
        }

        [Fact]
        public void Execute_WhenPassedPurchasesOnSameDate_ShouldFlattenThemIntoSingleTransaction()
        {
            // Arrange
            var asset = new Asset("test");
            var preMutated = new List<Transaction>
            {
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-01T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-01T02:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-01-02T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-02T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-03T01:00:00Z"), 1, 1, 1),
            };

            var expectedResult = new List<Transaction>
            {
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-01T01:00:00Z"), 2, 2, 2),
                new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-01-02T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-02T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-03T01:00:00Z"), 1, 1, 1),
            };

            IMutatorStrategy classUnderTest = new SameDayPurchaseMutatorStrategy();
            // Act
            var result = classUnderTest.Execute(preMutated);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Execute_WhenPassedTransactionsInJumbledDateOrder_ShouldReturnThemInAscendingDateOrder()
        {
            // Arrange
            var asset = new Asset("test");
            var preMutated = new List<Transaction>
            {
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-05T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-01-03T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-01T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-04T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-02T02:00:00Z"), 1, 1, 1),
            };

            IMutatorStrategy classUnderTest = new SameDayPurchaseMutatorStrategy();
            // Act
            var result = classUnderTest.Execute(preMutated);

            // Assert
            result.Should().BeInAscendingOrder(t => t.TransactionDate);
        }
    }
}