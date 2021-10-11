using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace SnAbp.Regulation
{
    [DependsOn(
        typeof(RegulationHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class RegulationConsoleApiClientModule : AbpModule
    {
        
    }
}
