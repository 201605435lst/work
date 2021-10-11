using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace SnAbp.Exam
{
    [DependsOn(
        typeof(ExamHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class ExamConsoleApiClientModule : AbpModule
    {
        
    }
}
