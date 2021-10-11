using Volo.Abp.Modularity;

namespace SnAbp.Report
{
    [DependsOn(
        typeof(ReportDomainSharedModule)
        )]
    public class ReportDomainModule : AbpModule
    {

    }
}
