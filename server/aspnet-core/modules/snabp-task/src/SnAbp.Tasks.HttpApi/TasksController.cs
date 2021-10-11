using SnAbp.Tasks.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Tasks
{
    public abstract class TasksController : AbpController
    {
        protected TasksController()
        {
            LocalizationResource = typeof(TasksResource);
        }
    }
}
