using SnAbp.Exam.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Exam
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(ExamEntityFrameworkCoreTestModule)
        )]
    public class ExamDomainTestModule : AbpModule
    {
        
    }
}
