using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.CostManagement.Entities;
using System;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.CostManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(CostManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class CostManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<CostManagementDbContext>(options =>
            {
                options.AddDefaultRepositories<ICostManagementDbContext>(true);

                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.Entity<PeopleCost>(x => x.DefaultWithDetailsFunc = q => q
                 .Include(x => x.Payee)
                 .Include(x => x.Professional));

                options.Entity<CostOther>(x => x.DefaultWithDetailsFunc = q => q
                .Include(y => y.Type));

                //MoneyList
                options.Entity<MoneyList>(x => x.DefaultWithDetailsFunc = q => q
                 .Include(x => x.Payee)
               .Include(y => y.Type));

                //Contract
                options.Entity<Contract>(x => x.DefaultWithDetailsFunc = q => q
                 .Include(x => x.Type)
               .Include(y => y.ContractRltFiles).ThenInclude(w => w.File));
            });
        }
    }
}