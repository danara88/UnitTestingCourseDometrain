using Xunit;

namespace AdvancesTechniques.Tests.Unit;

/**
* NOTES:
*
* Collection Fixtures: Are used when we want to share data
* with other collections.
*
*/
[CollectionDefinition("My awesome collection feature")]
public class TestCollectionFixture : ICollectionFixture<MyClassFixture>
{

}
