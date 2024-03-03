using CapitalGainCalculator.Common.Interfaces;
using CapitalGainCalculator.Common.Models;
using CapitalGainCalculator.CalculationEngine.Strategies.Rule;
using FluentAssertions;

namespace CapitalGainCalculator.CalculationEngine.Test.Strategies.Rule
{
    public class ThirtyDayRuleStrategy_MatchShould
    {
        [Fact]
        public void Match_WhenProvidedTransactionsEnumerableIsEmpty_ShouldReturnEmptyMatchCollection()
        {
            // Arrange
            var asset = new Asset("test");
            var context = new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-01T01:00:00Z"), 1, 1, 1);
            var transactions = Array.Empty<Transaction>();

            IRuleStrategy classUnderTest = new ThirtyDayRuleStrategy();

            // Act
            var result = classUnderTest.Match(transactions, context);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Match_WhenProvidedTransactionsAreBeforeOrOnSameDayAsContext_ShouldReturnEmptyMatchCollection()
        {
            // Arrange
            var asset = new Asset("test");
            var context = new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-02-01T01:00:00Z"), 1, 1, 1);
            var transactions = new List<Transaction>
            {
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-31T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-02-01T02:00:00Z"), 1, 1, 1),
            };

            IRuleStrategy classUnderTest = new ThirtyDayRuleStrategy();

            // Act
            var result = classUnderTest.Match(transactions, context);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Match_WhenProvidedTransactionsAreAfterSameDayAsContextButDisposals_ShouldReturnEmptyMatchCollection()
        {
            // Arrange
            var asset = new Asset("test");
            var context = new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-02-01T01:00:00Z"), 1, 1, 1);
            var transactions = new List<Transaction>
            {
                new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-02-02T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-02-03T02:00:00Z"), 1, 1, 1),
            };

            IRuleStrategy classUnderTest = new ThirtyDayRuleStrategy();

            // Act
            var result = classUnderTest.Match(transactions, context);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Match_WhenProvidedTransactionsContainOnePurchaseADayAfterContext_ShouldReturnTheDayAfterMatch()
        {
            // Arrange
            var asset = new Asset("test");
            var context = new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-02-01T01:00:00Z"), 1, 1, 1);
            var transactions = new List<Transaction>
            {
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-31T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-02-01T02:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-02-02T02:00:00Z"), 1, 1, 1),
            };

            IRuleStrategy classUnderTest = new ThirtyDayRuleStrategy();

            // Act
            var result = classUnderTest.Match(transactions, context);

            // Assert
            result.Should().ContainSingle();
        }

        [Fact]
        public void Match_WhenProvidedTransactionsContainOnePurchaseThirtyDaysAfterContext_ShouldReturnTheThirtyDayAfterMatch()
        {
            // Arrange
            var asset = new Asset("test");
            var context = new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-02-01T01:00:00Z"), 1, 1, 1);
            var transactions = new List<Transaction>
            {
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-31T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-03-03T02:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-03-04T02:00:00Z"), 1, 1, 1),
            };

            IRuleStrategy classUnderTest = new ThirtyDayRuleStrategy();

            // Act
            var result = classUnderTest.Match(transactions, context);

            // Assert
            result.Should().ContainSingle();
        }

        [Fact]
        public void Match_WhenProvidedTransactionsContainsSeveralTransactionsWithin30DaysButContextUnitsIsOnlyOne_ShouldReturnFirstMatch()
        {
            // Arrange
            var asset = new Asset("test");
            var context = new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-02-01T01:00:00Z"), 1, 1, 1);
            var transactions = new List<Transaction>
            {
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-31T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-02-02T02:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-02-03T02:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-02-04T02:00:00Z"), 1, 1, 1),
            };

            var expectedOutput = new []
            {
                new 
                {
                    TransactionDate = DateTimeOffset.Parse("2021-02-02T02:00:00Z")
                }
            };

            IRuleStrategy classUnderTest = new ThirtyDayRuleStrategy();

            // Act
            var result = classUnderTest.Match(transactions, context);

            // Assert
            result.Should().BeEquivalentTo(expectedOutput);
        }

        [Fact]
        public void Match_WhenProvidedTransactionsContainsSeveralTransactionsWithin30DaysButContextUnitsIs8_ShouldReturnFirstAndSecondMatches()
        {
            // Arrange
            var asset = new Asset("test");
            var context = new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-02-01T01:00:00Z"), 8, 1, 1);
            var transactions = new List<Transaction>
            {
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-31T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-02-02T02:00:00Z"), 5, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-02-03T02:00:00Z"), 5, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-02-04T02:00:00Z"), 1, 1, 1),
            };

            var expectedOutput = new []
            {
                new 
                {
                    TransactionDate = DateTimeOffset.Parse("2021-02-02T02:00:00Z"),
                    NumberOfShares = 5
                },
                new 
                {
                    TransactionDate = DateTimeOffset.Parse("2021-02-03T02:00:00Z"),
                    NumberOfShares = 5
                },
            };

            IRuleStrategy classUnderTest = new ThirtyDayRuleStrategy();

            // Act
            var result = classUnderTest.Match(transactions, context);

            // Assert
            result.Should().BeEquivalentTo(expectedOutput);
        }
    }
}