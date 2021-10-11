using SnAbp.Problem.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Problem
{
    public abstract class ProblemController : AbpController
    {
        protected ProblemController()
        {
            LocalizationResource = typeof(ProblemResource);
        }
    }
}
