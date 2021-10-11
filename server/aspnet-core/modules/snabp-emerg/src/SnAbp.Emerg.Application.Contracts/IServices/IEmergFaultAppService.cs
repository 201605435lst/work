using SnAbp.Emerg.Dtos;
using SnAbp.Emerg.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Emerg.IServices
{
    public interface IEmergFaultAppService : IApplicationService
    {
        Task<FaultDto> Get(Guid id);
        Task<PagedResultDto<FaultSimpleDto>> GetList(FaultSearchDto input);
        Task<FaultDto> Create(FaultCreateDto input);
        Task<FaultDto> Update(FaultUpdateDto input);

        /// <summary>
        /// 引用预案
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<EmergPlanRecordDto> ApplyEmergPlan(ApplyEmergPlanDto input);

        /// <summary>
        /// 故障处理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<FaultDto> Process(FaultProcessDto input);

 
    }
}
