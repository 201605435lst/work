using SnAbp.Tasks.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Tasks.IServices
{
    public interface ITasksTaskAppService : IApplicationService
    {
        Task<TaskDto> Create(TaskCreateDto input);
        Task<bool> Delete(Guid id);
        Task<TaskDto> Update(TaskUpdateDto input);
        Task<TaskDto> Feedback(TaskFeedBackDto input);
        Task<TaskExtendDto> Get(Guid id);
        Task<PagedResultDto<TaskDto>> GetList(TaskSearchDto input);
        Task<bool> UpdateState(TaskUpdateStateDto input);

        Task<Stream> Export(EduceDto input);
    }
}
