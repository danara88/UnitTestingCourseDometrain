using Xunit;

namespace AdvancesTechniques.Tests.Unit;

public class LongRunningTests
{
    [Fact(Timeout = 2000)] // If the test takes more than 2 seconds, test should fail
    public async Task SlowTest()
    {
        await Task.Delay(1000);
    }
}
