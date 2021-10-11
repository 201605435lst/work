using SnAbp.Material.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Material.IServices
{
   public interface IMaterialConstructionSectionAppService : IApplicationService
    {
        Task<ConstructionSectionDto> Create(ConstructionSectionCreateDto input);
        Task<bool> Delete(Guid Id);
        Task<ConstructionSectionDto> Update(ConstructionSectionUpdateDto input);
        Task<PagedResultDto<ConstructionSectionDto>> GetList(ConstructionSectionSearchDto input);
        Task<ConstructionSectionDto> Get(Guid id);
    }
}