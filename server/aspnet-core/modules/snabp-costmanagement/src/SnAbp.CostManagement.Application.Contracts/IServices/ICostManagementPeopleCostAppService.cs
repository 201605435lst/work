using SnAbp.CostManagement.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.CostManagement.IServices
{
    public interface ICostManagementPeopleCostAppService : IApplicationService
    {
        Task<PeopleCostDto> Create(PeopleCostCreateDto input);
        Task<bool> Delete(List<Guid> Ids);
        Task<PeopleCostDto> Update(PeopleCostUpdateDto input);
        Task<PagedResultDto<PeopleCostDto>> GetList(PeopleCostSearchDto input);
        Task<PeopleCostDto> Get(Guid id);
    }
}