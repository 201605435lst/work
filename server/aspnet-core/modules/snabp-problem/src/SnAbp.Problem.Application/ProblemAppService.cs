using SnAbp.Problem.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.Problem
{
    public abstract class ProblemAppService : ApplicationService
    {
        protected ProblemAppService()
        {
            LocalizationResource = typeof(ProblemResource);
            ObjectMapperContext = typeof(ProblemApplicationModule);
        }
    }
} 
