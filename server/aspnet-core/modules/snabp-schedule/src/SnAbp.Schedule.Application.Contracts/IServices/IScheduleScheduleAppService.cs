using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using SnAbp.Schedule.Dtos;
using Volo.Abp.Application.Dtos;
using SnAbp.Bpm.Dtos;

namespace SnAbp.Schedule.IServices
{
    public interface IScheduleScheduleAppService : IApplicationService
    {
        Task<ScheduleDto> Create(ScheduleCreateDto input);
        Task<bool> Delete(Guid id);
        Task<ScheduleDto> Update(ScheduleUpdateDto input);
        Task<PagedResultDto<ScheduleDto>> GetList(ScheduleSearchDto input);
        Task<ScheduleDto> Get(Guid id);
        Task<PagedResultDto<ScheduleSimpleDto>> GetByIds(List<Guid> id);
        //Task Import([FromForm] ImportData input);
        Task<Stream> Export(EduceScheduleDto input);

        Task<List<SingleFlowNodeDto>> GetFlowInfo(Guid workFlowId, Guid scheduleId);
    }
}
