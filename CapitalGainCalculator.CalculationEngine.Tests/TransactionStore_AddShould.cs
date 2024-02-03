using FluentAssertions;
using Moq.AutoMock;

namespace CapitalGainCalculator.CalculationEngine.Tests
{
    public class TransactionStore_AddShould
    {
        private readonly AutoMocker _mocker = new ();
        [Fact]
        public void Add_WhenGivenNullItem_ShouldThrowError()
        {
            // Arrange
            var classUnderTest = _mocker.CreateInstance<TransactionStore>();

            // Act
            var fn = () => classUnderTest.Add(null);

            // Assert
            fn.Should().ThrowExactly<ArgumentException>();
        }
    }
}