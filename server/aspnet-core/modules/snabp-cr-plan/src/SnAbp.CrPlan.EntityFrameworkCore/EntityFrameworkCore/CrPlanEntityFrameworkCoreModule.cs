using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Basic.EntityFrameworkCore;
using SnAbp.CrPlan.Entities;
using SnAbp.CrPlan.EntityFrameworkCore.EFCoreRepositories;
using SnAbp.CrPlan.Repositories;
using SnAbp.Identity.EntityFrameworkCore;
using System;
using Volo.Abp.Domain.Repositories;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.CrPlan.EntityFrameworkCore
{
    [DependsOn(
        typeof(CrPlanDomainModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(SnAbpIdentityEntityFrameworkCoreModule),
        typeof(BasicEntityFrameworkCoreModule)
    )]
    public class CrPlanEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<CrPlanDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddDefaultRepositories<ICrPlanDbContext>(true);
                options.Entity<MaintenanceWork>(x => x.DefaultWithDetailsFunc = q => q
                 .Include(x => x.Organization)
              );
                options.Entity<WorkTicket>(s => s.DefaultWithDetailsFunc = q =>
                    q.Include(x => x.TechnicalChecker)
                    .Include(x => x.SafetyDispatchChecker)
                );
                options.Entity<SkylightPlanRltWorkTicket>(s => s.DefaultWithDetailsFunc = q =>
                    q.Include(x => x.SkylightPlan)
                    .Include(x => x.WorkTicket)
                //.Include(x => x.WorkTicket).ThenInclude(y=>y.TechnicalChecker)
                //.Include(x => x.WorkTicket).ThenInclude(y=>y.SafetyDispatchChecker)
                );

                options.Entity<MaintenanceWorkRltFile>(s => s.DefaultWithDetailsFunc = q =>
                    q.Include(x => x.MaintenanceWork)
                    .Include(x => x.File)
                    .Include(x => x.RelateFile)
                );

                options.Entity<MaintenanceWorkRltSkylightPlan>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(x => x.SkylightPlan).ThenInclude(y => y.Railway)
                    .Include(x => x.MaintenanceWork)
                );

                options.Entity<MaintenanceWorkRltFile>(x => x.DefaultWithDetailsFunc = q =>
                    q.Include(x => x.MaintenanceWork)
                    .Include(y => y.File)
                    .Include(y => y.RelateFile)
                //q.Include(x => x.SkylightPlan).ThenInclude(y => y.Railway)
                );

                options.Entity<SkylightPlan>(x => x.DefaultWithDetailsFunc = q =>
                    q.Include(x => x.Railway)
                     .Include(x => x.WorkSites).ThenInclude(s => s.InstallationSite)
                //.Include(x=>x.SkylightPlanRltWorkTickets).ThenInclude(y=>y.SkylightPlan)
                );


                options.Entity<SkylightPlanRltInstallationSite>(x => x.DefaultWithDetailsFunc = q =>
                    q.Include(x => x.SkylightPlan).ThenInclude(x => x.WorkSites)

                );
                //options.Entity<SkylightPlanRltWorkTicket>(x => x.DefaultWithDetailsFunc = q =>
                //    q.Include(x => x.WorkTicket)
                //);
                //options.AddRepository<Organization, EFCoreOrganizationRespository>();
                //IRepository<YearMonthPlan, Guid>
            });

            context.Services.AddTransient<ICrPlanStatistialRepository, CrPlanStatisticalRepository>();
            //context.Services.AddScoped<IOrganizationRespository, EFCoreOrganizationRespository>();


        }
    }
}