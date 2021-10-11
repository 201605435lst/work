using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace SnAbp.File
{
    [DependsOn(
        typeof(File2HttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
    )]
    public class File2ConsoleApiClientModule : AbpModule
    {
    }
}