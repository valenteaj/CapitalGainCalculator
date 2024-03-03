using CapitalGainCalculator.Common.Interfaces;
using CapitalGainCalculator.Common.Models;
using CapitalGainCalculator.CalculationEngine.Strategies.Rule;
using FluentAssertions;

namespace CapitalGainCalculator.CalculationEngine.Test.Strategies.Rule
{
    public class SameDayRuleStrategy_CanExecuteShould
    {
        [Fact]
        public void CanExecute_WhenContextIsPurchaseAndTransactionsContainsAnotherPurchaseOnSameDate_ShouldReturnFalse()
        {
            // Arrange
            var asset = new Asset("test");
            var context = new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-01T01:00:00Z"), 1, 1, 1);
            var transactions = new List<Transaction>
            {
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-01T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-01T02:00:00Z"), 1, 1, 1),
            };

            IRuleStrategy classUnderTest = new SameDayRuleStrategy();

            // Act
            var result = classUnderTest.CanExecute(transactions, context);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CanExecute_WhenContextIsDisposalAndTransactionsContainsAnotherDisposalOnSameDate_ShouldReturnFalse()
        {
            // Arrange
            var asset = new Asset("test");
            var context = new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-01-01T01:00:00Z"), 1, 1, 1);
            var transactions = new List<Transaction>
            {
                new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-01-01T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-01-01T02:00:00Z"), 1, 1, 1),
            };

            IRuleStrategy classUnderTest = new SameDayRuleStrategy();

            // Act
            var result = classUnderTest.CanExecute(transactions, context);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CanExecute_WhenContextIsDisposalAndTransactionsContainsAPurchaseOnSameDate_ShouldReturnTrue()
        {
            // Arrange
            var asset = new Asset("test");
            var context = new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-01-01T01:00:00Z"), 1, 1, 1);
            var transactions = new List<Transaction>
            {
                new Transaction(TransactionType.Disposal, asset, DateTimeOffset.Parse("2021-01-01T01:00:00Z"), 1, 1, 1),
                new Transaction(TransactionType.Purchase, asset, DateTimeOffset.Parse("2021-01-01T02:00:00Z"), 1, 1, 1),
            };

            IRuleStrategy classUnderTest = new SameDayRuleStrategy();

            // Act
            var result = classUnderTest.CanExecute(transactions, context);

            // Assert
            result.Should().BeTrue();
        }
    }
}