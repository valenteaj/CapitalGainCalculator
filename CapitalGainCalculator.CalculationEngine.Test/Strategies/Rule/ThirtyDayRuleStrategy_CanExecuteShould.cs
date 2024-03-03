using CapitalGainCalculator.Common.Interfaces;
using CapitalGainCalculator.Common.Models;
using CapitalGainCalculator.CalculationEngine.Strategies.Rule;
using FluentAssertions;

namespace CapitalGainCalculator.CalculationEngine.Test.Strategies.Rule
{
    public class ThirtyDayRuleStrategy_CanExecuteShould
    {
        [Fact]
        public void CanExecute_WhenContextIsPurchaseAndTransactionsContainsAnotherPurchaseOnDayAfter_ShouldReturnFalse()
        {
            // Arrange
            var asset = new Asset("test");
            var context = new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-01T01:00:00Z"), 1, 1, 1);
            var transactions = new List<Transaction>
            {
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-01T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-01T02:00:00Z"), 1, 1, 1),
            };

            IRuleStrategy classUnderTest = new ThirtyDayRuleStrategy();

            // Act
            var result = classUnderTest.CanExecute(transactions, context);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CanExecute_WhenContextIsDisposalAndTransactionsContainsAnotherDisposalOnDayAfter_ShouldReturnFalse()
        {
            // Arrange
            var asset = new Asset("test");
            var context = new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-01-01T01:00:00Z"), 1, 1, 1);
            var transactions = new List<Transaction>
            {
                new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-01-01T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-01-01T02:00:00Z"), 1, 1, 1),
            };

            IRuleStrategy classUnderTest = new ThirtyDayRuleStrategy();

            // Act
            var result = classUnderTest.CanExecute(transactions, context);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CanExecute_WhenContextIsDisposalAndTransactionsContainsPurchasesOnDayBeforeAndSameDay_ShouldReturnFalse()
        {
            // Arrange
            var asset = new Asset("test");
            var context = new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-02-01T01:00:00Z"), 1, 1, 1);
            var transactions = new List<Transaction>
            {
                new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-01-31T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-31T02:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-02-01T02:00:00Z"), 1, 1, 1),
            };

            IRuleStrategy classUnderTest = new ThirtyDayRuleStrategy();

            // Act
            var result = classUnderTest.CanExecute(transactions, context);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CanExecute_WhenContextIsDisposalAndTransactionsContainsAPurchaseThirtyOneDaysAfter_ShouldReturnFalse()
        {
            // Arrange
            var asset = new Asset("test");
            var context = new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-01-01T01:00:00Z"), 1, 1, 1);
            var transactions = new List<Transaction>
            {
                new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-02-01T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-02-01T02:00:00Z"), 1, 1, 1),
            };

            IRuleStrategy classUnderTest = new ThirtyDayRuleStrategy();

            // Act
            var result = classUnderTest.CanExecute(transactions, context);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CanExecute_WhenContextIsDisposalAndTransactionsContainsAPurchaseOnDayAfter_ShouldReturnTrue()
        {
            // Arrange
            var asset = new Asset("test");
            var context = new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-01-01T01:00:00Z"), 1, 1, 1);
            var transactions = new List<Transaction>
            {
                new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-01-01T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-02T02:00:00Z"), 1, 1, 1),
            };

            IRuleStrategy classUnderTest = new ThirtyDayRuleStrategy();

            // Act
            var result = classUnderTest.CanExecute(transactions, context);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CanExecute_WhenContextIsDisposalAndTransactionsContainsAPurchaseThirtyDaysAfter_ShouldReturnTrue()
        {
            // Arrange
            var asset = new Asset("test");
            var context = new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-01-01T01:00:00Z"), 1, 1, 1);
            var transactions = new List<Transaction>
            {
                new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-01-31T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-31T02:00:00Z"), 1, 1, 1),
            };

            IRuleStrategy classUnderTest = new ThirtyDayRuleStrategy();

            // Act
            var result = classUnderTest.CanExecute(transactions, context);

            // Assert
            result.Should().BeTrue();
        }
    }
}