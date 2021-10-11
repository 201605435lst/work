using SnAbp.Emerg.Dtos;
using SnAbp.Emerg.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Emerg.IServices
{
    public interface IEmergPlanAppService:IApplicationService
    {
        Task<EmergPlanDto> Get(Guid id);
        Task<PagedResultDto<EmergPlanDto>> GetList(EmergPlanSearchDto input);
        Task<EmergPlanDto> Create(EmergPlanCreateDto input);
        Task<EmergPlanDto> Update(EmergPlanUpdateDto input);
        Task<bool> Delete(Guid id);
    }
}
