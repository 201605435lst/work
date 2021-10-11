using Microsoft.Extensions.DependencyInjection;
using SnAbp.EntityFrameworkCore;
using SnAbp.FileApprove.Entities;
using Volo.Abp.Modularity;

namespace SnAbp.FileApprove.EntityFrameworkCore
{
    [DependsOn(
        typeof(FileApproveDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class FileApproveEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<FileApproveDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                //                public DbSet<> FileApprove { get; set; }
                //public DbSet<FileApproveRltFlow> FileApproveRltFlow { get; set; }
                options.AddDefaultRepositories<IFileApproveDbContext>(true);
                options.Entity<FileApprove>(x => x.DefaultWithDetailsFunc = q => q

                 );
                options.Entity<FileApproveRltFlow>(x => x.DefaultWithDetailsFunc = q => q

                     );
            });
        }
    }
}