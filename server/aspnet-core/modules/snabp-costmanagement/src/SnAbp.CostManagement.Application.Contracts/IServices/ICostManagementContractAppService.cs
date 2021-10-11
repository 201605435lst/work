using SnAbp.CostManagement.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.CostManagement.IServices
{
    public interface ICostManagementContractAppService : IApplicationService
    {
        Task<CostContractDto> Create(CostContractCreateDto input);
        Task<bool> Delete(List<Guid> Ids);
        Task<CostContractDto> Update(CostContractUpdateDto input);
        Task<PagedResultDto<CostContractDto>> GetList(CostContractSearchDto input);
        Task<CostContractDto> Get(Guid id); 
       Task<BreakevenAnalysisDto> GetStatistics();
    }
}