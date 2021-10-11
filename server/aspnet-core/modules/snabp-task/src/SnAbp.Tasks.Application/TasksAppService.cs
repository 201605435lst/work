using SnAbp.Tasks.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.Tasks
{
    public abstract class TasksAppService : ApplicationService
    {
        protected TasksAppService()
        {
            LocalizationResource = typeof(TasksResource);
            ObjectMapperContext = typeof(TasksApplicationModule);
        }
    }
}
