using Volo.Abp.Modularity;

namespace SnAbp.Exam
{
    [DependsOn(
        typeof(ExamApplicationModule),
        typeof(ExamDomainTestModule)
        )]
    public class ExamApplicationTestModule : AbpModule
    {

    }
}
