using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Resource;
using SnAbp.Technology.Entities;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Technology.EntityFrameworkCore
{
    [DependsOn(
        typeof(TechnologyDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    //typeof(IdentityEntityFrameworkCoreModule)
    )]
    public class TechnologyEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<TechnologyDbContext>(options =>
            {
                options.AddDefaultRepositories<ITechnologyDbContext>(true);
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.Entity<MaterialPlan>(x => x.DefaultWithDetailsFunc = q => q
                  .Include(x => x.Materials).ThenInclude(y => y.Material).ThenInclude(s=>s.Type)
                    .Include(x => x.Creator)
                      .Include(x => x.MaterialPlanFlowInfos)
                      ); 
                options.Entity<ConstructInterface>(x => x.DefaultWithDetailsFunc = q => q
                .Include(x => x.Profession)
                .Include(x => x.Builder)
                .Include(x => x.Equipment).ThenInclude(y => y.Group)
                .Include(x => x.ConstructInterfaceInfos)
                );
                options.Entity<ConstructInterfaceInfo>(x => x.DefaultWithDetailsFunc = q => q
                 .Include(x => x.Marker)
                 .Include(x => x.Reformer)
                 .Include(x => x.Builder)
                 .Include(x => x.MarkFiles).ThenInclude(y => y.MarkFile));
            });
        }
    }
}