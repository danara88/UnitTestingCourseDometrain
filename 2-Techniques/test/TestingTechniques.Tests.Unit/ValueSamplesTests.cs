using FluentAssertions;
using Xunit;

namespace TestingTechniques.Tests.Unit;

/**
*
* NOTES:
*
* You don't need to test private methods directly in your tests.
* Private methods are tested by calling public or internal methods.
*/
public class ValueSamplesTests
{
    private readonly ValueSamples _sut = new();

    [Fact]
    public void StringAssertionExample()
    {
        var fullName = _sut.FullName;

        fullName.Should().Be("Nick Chapsas");
        fullName.Should().NotBeEmpty();
        fullName.Should().StartWith("Nick");
        fullName.Should().EndWith("Chapsas");
    }

    [Fact]
    public void NumberAssertionExample()
    {
        var age = _sut.Age;

        age.Should().Be(21);
        age.Should().BePositive();
        age.Should().BeGreaterThan(20);
        age.Should().BeLessThanOrEqualTo(21);
        age.Should().BeInRange(18, 60);
    }

    [Fact]
    public void DateAssertionExample()
    {
        var dateOfBirth = _sut.DateOfBirth;

        dateOfBirth.Should().Be(new(2000, 6, 9));
        dateOfBirth.Should().BeAfter(new(2000, 6, 8));
        dateOfBirth.Should().BeBefore(new(2000, 6, 10));
        dateOfBirth.Should().NotBe(new(2000, 7, 10));
    }

    [Fact]
    public void ObjectAssertionExample()
    {
        // This is not a value type, it is a reference type
        var expected = new User
        {
            FullName = "Nick Chapsas",
            Age = 21,
            DateOfBirth = new (2000, 6, 9)
        };

        var user = _sut.AppUser;

        // This will allow us compare reference types.
        // FluentAssertions will checks each property one by one in execution time.
        user.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void EnumerableObjectAssertionExample()
    {
        var expected = new User
        {
            FullName = "Nick Chapsas",
            Age = 21,
            DateOfBirth = new (2000, 6, 9)
        };

        var users = _sut.Users.As<User[]>();

        users.Should().ContainEquivalentOf(expected);
        users.Should().HaveCount(3);
        users.Should().Contain(x => x.FullName.StartsWith("Nick") && x.Age > 5);
    }

    [Fact]
    public void EnumerableNumbersAssertionExample()
    {
        // int is a value type
        var numbers = _sut.Numbers.As<int[]>();

        numbers.Should().Contain(5);
    }

    [Fact]
    public void ExceptionThrownAssertionExample()
    {
        var calculator = new Calculator();

        Action result = () => calculator.Divide(1, 0);

        result.Should()
            .Throw<DivideByZeroException>()
            .WithMessage("Attempted to divide by zero.");
    }

    [Fact]
    public void EventRaisedAssertionExample()
    {
        var monitorSubject = _sut.Monitor();

        _sut.RaiseExampleEvent();

        monitorSubject.Should().Raise("ExampleEvent");
    }

    [Fact]
    public void TestingInternalMembersExample()
    {
        // For internal members add this to the csproj of the project that you are testing
        //  <InternalsVisibleTo Include="TestingTechniques.Tests.Unit" />
        var number = _sut.InternalSecretNumber;

        number.Should().Be(42);
    }
}
