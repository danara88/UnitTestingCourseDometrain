using Xunit;
using Xunit.Abstractions;

namespace AdvancesTechniques.Tests.Unit;
/**
* NOTES:
* We know that when we execute a test in a collection, it will instanciate a new class.
* What happen if we need to share an instance with another test in a single collection?
* ClassFixture: A share context between tests. To use it we need to implement IClassFixture
*/
public class DefaultBehaviorTests : IClassFixture<MyClassFixture>
{
    // private readonly Guid _id = Guid.NewGuid();
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly MyClassFixture _fixture;

    public DefaultBehaviorTests(ITestOutputHelper testOutputHelper, MyClassFixture fixture)
    {
        _testOutputHelper = testOutputHelper;
        _fixture = fixture;
    }

    [Fact]
    public async Task ExampleTest1()
    {
        _testOutputHelper.WriteLine($"The Guid was: ", _fixture.Id);
        await Task.Delay(2000);
    }

    [Fact]
    public async Task ExampleTest2()
    {
        _testOutputHelper.WriteLine($"The Guid was: ", _fixture.Id);
         await Task.Delay(2000);
    }
}
