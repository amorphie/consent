using NetArchTest.Rules;

namespace amorphie.template.test;

public class DependencyCheck
{
    [Fact]
    public void DataToCoreCheck()
    {
        /*
        var result = Types.InCurrentDomain()
            .That()
            .ResideInNamespace("amorphie.template.core")
            .Should()
            .BeSealed()
            .GetResult()
            .IsSuccessful;

            Assert.True(result);
            */
    }
}