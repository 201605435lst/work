using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace SnAbp.Oa
{
    [DependsOn(
        typeof(OaHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class OaConsoleApiClientModule : AbpModule
    {
        
    }
}
