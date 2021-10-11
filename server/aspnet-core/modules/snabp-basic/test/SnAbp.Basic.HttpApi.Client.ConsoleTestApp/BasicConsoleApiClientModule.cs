using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace SnAbp.Basic
{
    [DependsOn(
        typeof(BasicHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class BasicConsoleApiClientModule : AbpModule
    {
        
    }
}
