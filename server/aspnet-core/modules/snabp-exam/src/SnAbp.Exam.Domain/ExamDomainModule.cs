using Volo.Abp.Modularity;

namespace SnAbp.Exam
{
    [DependsOn(
        typeof(ExamDomainSharedModule)
        )]
    public class ExamDomainModule : AbpModule
    {

    }
}
