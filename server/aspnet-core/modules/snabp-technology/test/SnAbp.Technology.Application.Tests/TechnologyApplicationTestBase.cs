using Xunit;

namespace SnAbp.Technology
{
    /* Inherit from this class for your application layer tests.
     * See SampleAppService_Tests for example.
     */
    public abstract class TechnologyApplicationTestBase : TechnologyTestBase<TechnologyApplicationTestModule>
    {
        [Fact]
        public void Test()
        {

        }
    }
}