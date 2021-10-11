using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Basic.Entities;
//using SnAbp.Basic.Entities;
//using SnAbp.Basic.Repositories;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Basic.EntityFrameworkCore
{
    [DependsOn(
        typeof(BasicDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class BasicEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<BasicDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddDefaultRepositories<IBasicDbContext>(true);
                //options.AddRepository<Organization, EFCoreOrganizationRespository>();

                options.Entity<Railway>(s => s.DefaultWithDetailsFunc = x => x
                    .Include(s => s.RailwayRltOrganizations).ThenInclude(z => z.Organization)
                );

                options.Entity<StationRltRailway>(s => s.DefaultWithDetailsFunc = x => x
                    .Include(x => x.Railway)
                    .Include(x => x.Station)
                );

                options.Entity<InstallationSite>(s => s.DefaultWithDetailsFunc = x => x
                    .Include(x => x.Organization)
                    .Include(x => x.Station)
                    .Include(x => x.Railway).ThenInclude(x=>x.RailwayRltOrganizations).ThenInclude(x=>x.Organization)
                    .Include(x => x.Type)
                    .Include(x => x.Children)
                    .Include(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.Parent)
                );

                options.Entity<RailwayRltOrganization>(s => s.DefaultWithDetailsFunc = x => x
                    .Include(s => s.Railway)
                    .Include(s => s.Organization)
                    
                );
            });

            //context.Services.AddScoped<IOrganizationRespository, EFCoreOrganizationRespository>();


        }
    }
}