using SnAbp.Project.Dtos;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Project.IServices
{
    public interface IProjectAppServices : IApplicationService
    {
        Task<ProjectDto> Create(ProjectCreateDto input);
        Task<bool> Delete(Guid id);
        Task<ProjectDto> Update(ProjectUpdateDto input);
        Task<ProjectDto> Get(Guid id);
        Task<PagedResultDto<ProjectDto>> GetList(ProjectSearchDto input);
        Task<bool> UpdateState(ProjectUpdateStateDto input);

        Task<Stream> Export(ExportDto input);
    }
}
