using SnAbp.Project.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.Project
{
    public abstract class ProjectAppService : ApplicationService
    {
        protected ProjectAppService()
        {
            LocalizationResource = typeof(ProjectResource);
            ObjectMapperContext = typeof(ProjectApplicationModule);
        }
    }
}
