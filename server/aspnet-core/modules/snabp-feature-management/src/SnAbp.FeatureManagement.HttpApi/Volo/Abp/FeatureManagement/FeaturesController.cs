using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.FeatureManagement
{
    [RemoteService(Name = FeatureManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("featureManagement")]
    [ControllerName("AppFeatures")]
    [Route("api/feature-management/features")]
    public class FeaturesController : AbpController, IAppFeatureAppService
    {
        protected IAppFeatureAppService FeatureAppService { get; }

        public FeaturesController(IAppFeatureAppService featureAppService)
        {
            FeatureAppService = featureAppService;
        }

        [HttpGet]
        public virtual Task<FeatureListDto> GetAsync(string providerName, string providerKey)
        {
            return FeatureAppService.GetAsync(providerName, providerKey);
        }

        [HttpPut]
        public virtual Task UpdateAsync(string providerName, string providerKey, UpdateFeaturesDto input)
        {
            return FeatureAppService.UpdateAsync(providerName, providerKey, input);
        }
    }
}