using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace SnAbp.Emerg
{
    [DependsOn(
        typeof(EmergHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class EmergConsoleApiClientModule : AbpModule
    {
        
    }
}
