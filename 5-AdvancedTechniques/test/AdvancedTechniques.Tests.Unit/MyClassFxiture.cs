namespace AdvancesTechniques.Tests.Unit;

public class MyClassFixture
{
    public Guid Id { get; } = Guid.NewGuid();

    // This constructor will execute at the very begining
    // public MyClassFxiture()
    // {
    //     // Here we can put any code that we wanna share with other tests
    // }

    // public void Dispose()
    // {
    //     // Here put all the code that you need to cleanup when all the tests are done
    //     throw new NotImplementedException();
    // }
}
