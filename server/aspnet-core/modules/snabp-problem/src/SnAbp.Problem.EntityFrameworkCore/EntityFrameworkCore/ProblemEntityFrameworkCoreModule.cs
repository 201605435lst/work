using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
//using SnAbp.Problem.Entities;
//using SnAbp.Problem.Repositories;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Problem.EntityFrameworkCore
{
    [DependsOn(
        typeof(ProblemDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class ProblemEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ProblemDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */

                options.Entity<Entities.Problem>(x => x.DefaultWithDetailsFunc = q => q
                      .Include(x => x.ProblemRltProblemCategories).ThenInclude(r => r.ProblemCategory)
                  );

                options.AddDefaultRepositories<IProblemDbContext>(true);
                //options.AddRepository<Organization, EFCoreOrganizationRespository>();
            });

            //context.Services.AddScoped<IOrganizationRespository, EFCoreOrganizationRespository>();


        }
    }
}