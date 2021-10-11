using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace SnAbp.Problem
{
    [DependsOn(
        typeof(ProblemHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class ProblemConsoleApiClientModule : AbpModule
    {
        
    }
}
