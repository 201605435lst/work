using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Emerg.Entities;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Emerg.EntityFrameworkCore
{
    [DependsOn(
        typeof(EmergDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class EmergEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<EmergDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddDefaultRepositories<IEmergDbContext>(true);

                options.Entity<Fault>(x => x.DefaultWithDetailsFunc = q => q
                        .Include(x => x.FaultRltComponentCategories).ThenInclude(r => r.ComponentCategory)
                        //.Include(x => x.FaultRltEquipments).ThenInclude(r => r.Equipment)
                        .Include(x => x.FaultRltEquipments).ThenInclude(r => r.Equipment).ThenInclude(y => y.Group)
                        .Include(x => x.Level)
                        .Include(x => x.Station)
                        .Include(x => x.EmergPlanRecord).ThenInclude(r => r.ProcessRecords)
                    );
                options.Entity<EmergPlan>(x => x.DefaultWithDetailsFunc = y => y
                .Include(x => x.EmergPlanRltComponentCategories).ThenInclude(r => r.ComponentCategory)
                .Include(x => x.Level)
                .Include(x => x.EmergPlanRltFiles).ThenInclude(r => r.File)
                );



            });

            //context.Services.AddScoped<IOrganizationRespository, EFCoreOrganizationRespository>();

        }
    }
}