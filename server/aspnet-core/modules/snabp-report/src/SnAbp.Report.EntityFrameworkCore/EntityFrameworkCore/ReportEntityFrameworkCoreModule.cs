using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Report.Entities;
using System;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Report.EntityFrameworkCore
{
    [DependsOn(
        typeof(ReportDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class ReportEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ReportDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddRepository<ReportRltFile, EfCoreRepository<IReportDbContext, ReportRltFile, Guid>>();
                options.AddRepository<ReportRltUser, EfCoreRepository<IReportDbContext, ReportRltUser, Guid>>();
                options.AddRepository<Report, EfCoreRepository<IReportDbContext, Report, Guid>>();

                options.Entity<Report>(x => x.DefaultWithDetailsFunc = q => q
                   .Include(x => x.Organization)
                   .Include(x=>x.ReportRltUsers).ThenInclude(y=>y.User)
                   .Include(x => x.ReportRltFiles).ThenInclude(y => y.File)
               );


            });
        }
    }
}