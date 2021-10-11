using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace SnAbp.Project.Samples
{
    public class SampleAppService_Tests : ProjectApplicationTestBase
    {
        private readonly IProjectSampleAppService _sampleAppService;

        public SampleAppService_Tests()
        {
            _sampleAppService = GetRequiredService<IProjectSampleAppService>();
        }

        [Fact]
        public async Task GetAsync()
        {
            var result = await _sampleAppService.GetAsync();
            result.Value.ShouldBe(42);
        }

        [Fact]
        public async Task GetAuthorizedAsync()
        {
            var result = await _sampleAppService.GetAuthorizedAsync();
            result.Value.ShouldBe(42);
        }
    }
}
