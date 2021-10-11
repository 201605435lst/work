using SnAbp.Schedule.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SnAbp.Schedule.IServices
{
    public interface IScheduleScheduleFlowTemplateAppService : IApplicationService
    {
        Task<ScheduleFlowTemplateDto> Create(ScheduleFlowTemplateCreateDto input);
        //Task<ScheduleFlowTemplateDto> Update(ScheduleFlowTemplateUpdateDto input);
    }
}
