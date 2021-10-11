using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace SnAbp.Report
{
    [DependsOn(
        typeof(ReportHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class ReportConsoleApiClientModule : AbpModule
    {
        
    }
}
