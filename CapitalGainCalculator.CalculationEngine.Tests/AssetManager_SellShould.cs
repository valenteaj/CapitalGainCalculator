using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;
using FluentAssertions;
using Moq.AutoMock;

namespace CapitalGainCalculator.CalculationEngine.Tests
{
    public class AssetManager_SellShould
    {
        private AutoMocker _mocker = new();

        [Fact]
        public void Sell_WhenNoUsefulParametersPassed_ShouldThowError()
        {
            // Arrange
            var classUnderTest = _mocker.CreateInstance<AssetManager>();

            // Act
            var fn = () => classUnderTest.Sell(null, 0, 0, 0);

            // Assert
            fn.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void Sell_WhenGivenValidArguments_ShouldRegisterAndReturnTransaction()
        {
            // Arrange
            var timestamp = DateTimeOffset.MinValue;
            var asset = new Asset("test-asset");
            var expectedTransaction = new Transaction(TransactionType.Disposal, asset, timestamp, 0, 0, 0);
            
            var ledger = _mocker.GetMock<ILedger>();
            ledger.Setup(_ => _.RegisterTransaction(expectedTransaction));
            var classUnderTest = _mocker.CreateInstance<AssetManager>();

            // Act
            var result = classUnderTest.Sell(asset, 0, 0, 0, timestamp);

            // Assert
            result.Should().BeEquivalentTo(expectedTransaction);
            ledger.VerifyAll();
        }
    }
}