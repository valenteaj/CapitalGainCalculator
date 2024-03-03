using CapitalGainCalculator.Common.Interfaces;
using CapitalGainCalculator.Common.Models;
using CapitalGainCalculator.CalculationEngine.Strategies.Rule;
using FluentAssertions;

namespace CapitalGainCalculator.CalculationEngine.Test.Strategies.Rule
{
    public class Section104HoldingRuleStrategy_MatchShould
    {
        [Fact]
        public void Match_WhenProvidedTransactionsEnumerableIsEmpty_ShouldReturnEmptyMatchCollection()
        {
            // Arrange
            var asset = new Asset("test");
            var context = new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-01T01:00:00Z"), 1, 1, 1);
            var transactions = Array.Empty<Transaction>();

            IRuleStrategy classUnderTest = new Section104HoldingRuleStrategy();

            // Act
            var result = classUnderTest.Match(transactions, context);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Match_WhenProvidedTransactionsAreAfterContext_ShouldReturnEmptyMatchCollection()
        {
            // Arrange
            var asset = new Asset("test");
            var context = new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-02-01T01:00:00Z"), 1, 1, 1);
            var transactions = new List<Transaction>
            {
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-02-02T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-03-02T02:00:00Z"), 1, 1, 1),
            };

            IRuleStrategy classUnderTest = new Section104HoldingRuleStrategy();

            // Act
            var result = classUnderTest.Match(transactions, context);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Match_WhenProvidedTransactionsAreOnSameDayOrBeforeContextButDisposals_ShouldReturnEmptyMatchCollection()
        {
            // Arrange
            var asset = new Asset("test");
            var context = new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-02-01T01:00:00Z"), 1, 1, 1);
            var transactions = new List<Transaction>
            {
                new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-01-31T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-02-01T02:00:00Z"), 1, 1, 1),
            };

            IRuleStrategy classUnderTest = new Section104HoldingRuleStrategy();

            // Act
            var result = classUnderTest.Match(transactions, context);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Match_WhenProvidedTransactionsContainPurchaseBeforeAndOnSameDayAsContext_ShouldReturnBothMatches()
        {
            // Arrange
            var asset = new Asset("test");
            var context = new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-02-01T01:00:00Z"), 1, 1, 1);
            var transactions = new List<Transaction>
            {
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-01T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-02-01T02:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-03-01T02:00:00Z"), 1, 1, 1),
            };

            IRuleStrategy classUnderTest = new Section104HoldingRuleStrategy();

            // Act
            var result = classUnderTest.Match(transactions, context);

            // Assert
            result.Count().Should().Be(2);
        }
    }
}