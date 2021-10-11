using SnAbp.Schedule.Dtos;
using SnAbp.Schedule.Entities;
using SnAbp.Schedule.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Schedule.Services
{
    public class ScheduleScheduleFlowTemplateAppService : ScheduleAppService, IScheduleScheduleFlowTemplateAppService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<ScheduleFlowTemplate, Guid> _scheduleFlowTemplateRepository;

        public ScheduleScheduleFlowTemplateAppService(
            IRepository<ScheduleFlowTemplate, Guid> scheduleFlowTemplateRepository,
            IGuidGenerator guidGenerator
            )
        {
            _scheduleFlowTemplateRepository = scheduleFlowTemplateRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task<ScheduleFlowTemplateDto> Create(ScheduleFlowTemplateCreateDto input)
        {
            await _scheduleFlowTemplateRepository.DeleteAsync(x => x.Id != null);

            var scheduleFlowTemplate = new ScheduleFlowTemplate(_guidGenerator.Create())
            {
                WorkflowTemplateId = input.WorkflowTemplateId,
            };
            await _scheduleFlowTemplateRepository.InsertAsync(scheduleFlowTemplate);

            return await Task.FromResult(ObjectMapper.Map<ScheduleFlowTemplate, ScheduleFlowTemplateDto>(scheduleFlowTemplate));
        }

    }
}
