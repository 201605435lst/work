using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SnAbp.Project.Samples;

namespace SnAbp.Project
{
    public class ProjectSampleAppService : ProjectAppService, IProjectSampleAppService
    {
        public Task<ProjectSampleDto> GetAsync()
        {
            return Task.FromResult(
                new ProjectSampleDto
                {
                    Value = 42
                }
            );
        }

        [Authorize]
        public Task<ProjectSampleDto> GetAuthorizedAsync()
        {
            return Task.FromResult(
                new ProjectSampleDto
                {
                    Value = 42
                }
            );
        }
    }
}