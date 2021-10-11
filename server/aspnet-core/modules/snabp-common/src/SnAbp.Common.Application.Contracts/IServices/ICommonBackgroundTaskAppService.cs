using SnAbp.Common.Dtos;
using SnAbp.Common.Dtos.Task;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Common.IServices
{
    public interface ICommonBackgroundTaskAppService : IApplicationService
    {
        Task<BackgroundTaskDto> Get(string key);
        Task<BackgroundTaskDto> Create(BackgroundTaskDto input);
        Task<BackgroundTaskDto> Update(BackgroundTaskDto input);
        Task<bool> Done(string key);
        Task<bool> Cancel(string key);
    }
}
