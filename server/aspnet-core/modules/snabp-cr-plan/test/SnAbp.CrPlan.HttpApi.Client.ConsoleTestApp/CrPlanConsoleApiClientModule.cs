using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace SnAbp.CrPlan
{
    [DependsOn(
        typeof(CrPlanHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class CrPlanConsoleApiClientModule : AbpModule
    {
        
    }
}
