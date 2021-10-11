using Xunit;

namespace SnAbp.File
{
    /* Inherit from this class for your domain layer tests.
     * See SampleManager_Tests for example.
     */
    public abstract class File2DomainTestBase : File2TestBase<File2DomainTestModule>
    {
        [Fact]
        public void CreatePreSign()
        {
            var a = 1;
        }
    }
}