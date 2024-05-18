using Xunit;
using Xunit.Abstractions;

namespace AdvancesTechniques.Tests.Unit;

/**
* NOTES:
* CollectionFictures: Is the context that we want to share between other collections.
* To achive that we have to implement the decorator [Collection("My awesome collection feature")]
* Execution flow: Constructor fixture execute first because it will be share accross every fixture collection later.
* Then, it will be executed the constructor of the collection.
*/
[Collection("My awesome collection feature")]
public class CollectionFixturesBehaviorTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly MyClassFixture _fixture;

    public CollectionFixturesBehaviorTests(ITestOutputHelper testOutputHelper, MyClassFixture fixture)
    {
        _testOutputHelper = testOutputHelper;
        _fixture = fixture;
    }

    [Fact]
    public void ExampleTest1()
    {
        _testOutputHelper.WriteLine($"The Guid was: ", _fixture.Id);
    }

    [Fact]
    public void ExampleTest2()
    {
        _testOutputHelper.WriteLine($"The Guid was: ", _fixture.Id);
    }
}

[Collection("My awesome collection feature")]
public class CollectionFixturesBehaviorTestsAgain
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly MyClassFixture _fixture;

    public CollectionFixturesBehaviorTestsAgain(ITestOutputHelper testOutputHelper, MyClassFixture fixture)
    {
        _testOutputHelper = testOutputHelper;
        _fixture = fixture;
    }

    [Fact]
    public void ExampleTest1()
    {
        _testOutputHelper.WriteLine($"The Guid was: ", _fixture.Id);
    }

    [Fact]
    public void ExampleTest2()
    {
        _testOutputHelper.WriteLine($"The Guid was: ", _fixture.Id);
    }
}
