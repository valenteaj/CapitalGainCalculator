using CapitalGainCalculator.CalculationEngine.Strategies.Mutator;
using CapitalGainCalculator.Common.Interfaces;
using CapitalGainCalculator.Common.Models;
using FluentAssertions;

namespace CapitalGainCalculator.CalculationEngine.Test.Strategies.Mutator
{
    public class SameDayPurchaseMutatorStrategy_CanExecuteShould
    {
        [Fact]
        public void CanExecute_AlwaysReturnsTrue()
        {
            IMutatorStrategy classUnderTest = new SameDayPurchaseMutatorStrategy();
            var result = classUnderTest.CanExecute(Array.Empty<Transaction>());
            result.Should().BeTrue();
        }
    }
}