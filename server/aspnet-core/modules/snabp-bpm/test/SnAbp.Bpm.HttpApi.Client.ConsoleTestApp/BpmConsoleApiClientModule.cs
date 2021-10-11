using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace SnAbp.Bpm
{
    [DependsOn(
        typeof(BpmHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class BpmConsoleApiClientModule : AbpModule
    {
        
    }
}
