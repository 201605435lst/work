using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;
using SnAbp.Quality.Entities;
using System;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Quality.EntityFrameworkCore
{
    [DependsOn(
        typeof(QualityDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class QualityEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<QualityDbContext>(options =>
            {
                options.AddDefaultRepositories<IQualityDbContext>(true);
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddRepository<QualityProblemRltCcUser, EfCoreRepository<QualityDbContext, QualityProblemRltCcUser, Guid>>();
                options.AddRepository<QualityProblemRltEquipment, EfCoreRepository<QualityDbContext, QualityProblemRltEquipment, Guid>>();
                options.AddRepository<QualityProblemRltFile, EfCoreRepository<QualityDbContext, QualityProblemRltFile, Guid>>();
                options.Entity<QualityProblemLibrary>(x => x.DefaultWithDetailsFunc = q => q
                       .Include(x => x.Profession)
                       .Include(x => x.Scops).ThenInclude(r => r.Scop)
                   );

                options.Entity<QualityProblem>(x => x.DefaultWithDetailsFunc = q => q
                       .Include(x => x.Checker)
                       .Include(x => x.Profession)
                       .Include(x => x.CheckUnit)
                       .Include(x => x.ResponsibleUser)
                       .Include(x => x.ResponsibleOrganization)
                       .Include(x => x.Verifier)
                       .Include(x => x.Files).ThenInclude(y => y.File)
                       .Include(x => x.CcUsers).ThenInclude(y => y.CcUser)
                       .Include(x => x.Equipments).ThenInclude(y => y.Equipment).ThenInclude(r=>r.Group)
                   );

                options.Entity<QualityProblemRecord>(x => x.DefaultWithDetailsFunc = q => q
                      .Include(x => x.User)
                      .Include(x => x.Files).ThenInclude(y => y.File)
                  );
            });
        }
    }
}