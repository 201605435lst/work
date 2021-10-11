using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SnAbp.Project.Samples
{
    public interface IProjectSampleAppService : IApplicationService
    {
        Task<ProjectSampleDto> GetAsync();

        Task<ProjectSampleDto> GetAuthorizedAsync();
    }
}
