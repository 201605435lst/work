using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace SnAbp.StdBasic
{
    [DependsOn(
        typeof(StdBasicHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class StdBasicConsoleApiClientModule : AbpModule
    {
        
    }
}
