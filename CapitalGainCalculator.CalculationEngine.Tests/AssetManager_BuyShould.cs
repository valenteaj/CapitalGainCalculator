using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;
using FluentAssertions;
using Moq.AutoMock;

namespace CapitalGainCalculator.CalculationEngine.Tests
{
    public class AssetManager_BuyShould
    {
        private AutoMocker _mocker = new();

        [Fact]
        public void Buy_WhenNoUsefulParametersPassed_ShouldThowError()
        {
            // Arrange
            var classUnderTest = _mocker.CreateInstance<AssetManager>();

            // Act
            var fn = () => classUnderTest.Buy(null, 0, 0, 0);

            // Assert
            fn.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void Buy_WhenGivenValidArguments_ShouldRegisterAndReturnTransaction()
        {
            // Arrange
            var timestamp = DateTimeOffset.MinValue;
            var asset = new Asset("test-asset");
            var expectedTransaction = new Transaction(TransactionType.Purchase, asset, timestamp, 0, 0, 0);

            var ledger = _mocker.GetMock<ILedger>();
            ledger.Setup(_ => _.RegisterTransaction(expectedTransaction));
            var classUnderTest = _mocker.CreateInstance<AssetManager>();

            // Act
            var result = classUnderTest.Buy(asset, 0, 0, 0, timestamp);

            // Assert
            result.Should().BeEquivalentTo(expectedTransaction);
            ledger.VerifyAll();
        }
    }
}