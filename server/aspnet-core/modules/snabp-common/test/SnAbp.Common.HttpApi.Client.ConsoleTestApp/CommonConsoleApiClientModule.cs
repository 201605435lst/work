using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace SnAbp.Common
{
    [DependsOn(
        typeof(CommonHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class CommonConsoleApiClientModule : AbpModule
    {
        
    }
}
