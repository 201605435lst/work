using SnAbp.Project.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Project.IServices
{
    public interface IProjectArchivesCategoryAppService : IApplicationService
    {
        Task<ArchivesCategoryDto> Create(ArchivesCategoryCreateDto input);
        Task<bool> Delete(Guid id);
        Task<ArchivesCategoryDto> Update(ArchivesCategoryUpdateDto input);
        Task<bool> Lock(ArchivesCategoryLockDto input);
        Task<ArchivesCategoryDto> Get(Guid id);
        Task<List<ArchivesCategoryDto>> GetList(ArchivesSearchDto input);
    }
}
