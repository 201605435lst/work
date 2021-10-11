using SnAbp.CostManagement.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.CostManagement.IServices
{
   public interface ICostManagementMoneyListAppService : IApplicationService
    {
        Task<MoneyListDto> Create(MoneyListCreateDto input);
        Task<bool> Delete(List<Guid> Ids);
        Task<MoneyListDto> Update(MoneyListUpdateDto input);
        Task<PagedResultDto<MoneyListDto>> GetList(MoneyListSearchDto input);
        Task<MoneyListDto> Get(Guid id);
        Task<MoneyListStatisticsDto> GetStatistics(MoneyListSearchDto input);
    }
}