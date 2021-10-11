using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace SnAbp.Project.Samples
{
    [RemoteService]
    [Route("api/Project/sample")]
    public class SampleController : ProjectController, IProjectSampleAppService
    {
        private readonly IProjectSampleAppService _sampleAppService;

        public SampleController(IProjectSampleAppService sampleAppService)
        {
            _sampleAppService = sampleAppService;
        }

        [HttpGet]
        public async Task<ProjectSampleDto> GetAsync()
        {
            return await _sampleAppService.GetAsync();
        }

        [HttpGet]
        [Route("authorized")]
        [Authorize]
        public async Task<ProjectSampleDto> GetAuthorizedAsync()
        {
            return await _sampleAppService.GetAsync();
        }
    }
}
