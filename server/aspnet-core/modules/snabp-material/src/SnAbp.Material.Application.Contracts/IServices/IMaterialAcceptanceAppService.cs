using SnAbp.Material.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Material.IServices
{
   public interface IMaterialAcceptanceAppService:IApplicationService
    {
        Task<MaterialAcceptanceDto> Get(Guid id);
        Task<PagedResultDto<MaterialAcceptanceDto>> GetList(MaterialAcceptanceSearchDto input);
        Task<MaterialAcceptanceDto> Create(MaterialAcceptanceCreateDto input);
        Task<MaterialAcceptanceDto> Update(MaterialAcceptanceUpdateDto input);
        Task<bool> Delete(Guid Id);
    }
}
