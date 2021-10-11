using SnAbp.CostManagement.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.CostManagement.IServices
{
   public interface ICostManagementCostOtherAppService : IApplicationService
    {
        Task<CostOtherDto> Create(CostOtherCreateDto input);
        Task<bool> Delete(List<Guid> Ids);
        Task<CostOtherDto> Update(CostOtherUpdateDto input);
        Task<PagedResultDto<CostOtherDto>> GetList(CostOtherSearchDto input);
        Task<CostOtherDto> Get(Guid id);
    }
}