using FluentAssertions;
using Xunit;

namespace TestingTechniques.Tests.Unit;

/**
*
* NOTES:
*
* xUnit will create a instance of the class for each of your tests
* To setup the test, you can use the constructor.
* To cleanup your test, you can use Dispose from IDisposable interface.
*
* If we want to test async code for setup and cleanup we can implement IAsyncLifetime.
* Execution order: ctor -> InitializeAsync -> Fact -> DisposeAsync
* The constructor always execute always first.
*
* Parameterizing: Allow you to define a set of parameters at the attr level.
*
* To ignore a Fact you can use this [Fact(Skip = "Skip this)] or [Theory(Skip = "Skip this)]
*/

public class CalculatorTests
{
    private readonly Calculator _sut = new(); // sut = System Under Test

    [Theory]
    [InlineData(5, 5, 10)]
    [InlineData(-5, 5, 0)]
    [InlineData(-15, -5, -20)]
    public void Add_ShouldAddTwoNumbers_WhenTwoNumbersAreIntegers(int a, int b, int expected)
    {
        // Act
        var result = _sut.Add(a, b);
        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(5, 5, 0)]
    [InlineData(15, 5, 10)]
    [InlineData(-5, -5, 0)]
    [InlineData(-15, -5, -10)]
    [InlineData(5, 10, -5)]
    public void Add_ShouldSubstractTwoNumbers_WhenTwoNumbersAreIntegers(int a, int b, int expected)
    {
        // Act
        var result = _sut.Subtract(a, b);
        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(5, 5, 25)]
    [InlineData(50, 0, 0)]
    [InlineData(-5, 5, -25)]
    public void Multiply_ShouldMultiplyTwoNumbers_WhenTwoNumbersAreIntegers(int a, int b, int expected)
    {
        // Act
        var result = _sut.Multiply(a, b);
        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(5, 5, 1)]
    [InlineData(15, 5, 3)]
    public void Divide_ShouldDivideTwoNumbers_WhenTwoNumbersAreIntegers(int a, int b, int expected)
    {
        // Act
        var result = _sut.Divide(a, b);
        // Assert
        result.Should().Be(expected);
    }
}
