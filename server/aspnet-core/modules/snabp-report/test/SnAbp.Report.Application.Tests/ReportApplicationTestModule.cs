using Volo.Abp.Modularity;

namespace SnAbp.Report
{
    [DependsOn(
        typeof(ReportApplicationModule),
        typeof(ReportDomainTestModule)
        )]
    public class ReportApplicationTestModule : AbpModule
    {

    }
}
