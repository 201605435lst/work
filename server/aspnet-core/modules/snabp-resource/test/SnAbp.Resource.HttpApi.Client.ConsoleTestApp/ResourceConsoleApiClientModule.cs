using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace SnAbp.Resource
{
    [DependsOn(
        typeof(ResourceHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class ResourceConsoleApiClientModule : AbpModule
    {
        
    }
}
