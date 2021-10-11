using SnAbp.Project.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Project
{
    public abstract class ProjectController : AbpController
    {
        protected ProjectController()
        {
            LocalizationResource = typeof(ProjectResource);
        }
    }
}
