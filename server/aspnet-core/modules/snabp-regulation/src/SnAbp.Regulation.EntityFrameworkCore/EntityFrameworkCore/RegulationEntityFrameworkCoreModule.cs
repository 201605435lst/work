using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.File.EntityFrameworkCore;
using SnAbp.Identity;
using SnAbp.Regulation.Entities;
using System;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Regulation.EntityFrameworkCore
{
    [DependsOn(
        typeof(RegulationDomainModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(FileEntityFrameworkCoreModule)
    )]
    public class RegulationEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<RegulationDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddRepository<Label, EfCoreRepository<IRegulationDbContext, Label, Guid>>();
                options.AddRepository<Institution, EfCoreRepository<IRegulationDbContext, Institution, Guid>>();
                options.AddRepository<InstitutionRltFile, EfCoreRepository<IRegulationDbContext, InstitutionRltFile, Guid>>();
                options.AddRepository<InstitutionRltLabel, EfCoreRepository<IRegulationDbContext, InstitutionRltLabel, Guid>>();
                options.AddRepository<InstitutionRltEdition, EfCoreRepository<IRegulationDbContext, InstitutionRltEdition, Guid>>();
                options.AddRepository<InstitutionRltAuthority, EfCoreRepository<IRegulationDbContext, InstitutionRltAuthority, Guid>>();
                options.AddRepository<InstitutionRltFlow, EfCoreRepository<IRegulationDbContext, InstitutionRltFlow, Guid>>();



                options.Entity<Institution>(a => a.DefaultWithDetailsFunc = b => b
                .Include(c => c.Organization)
                .Include(c => c.InstitutionRltAuthorities)
                .Include(c => c.InstitutionRltFiles).ThenInclude(d => d.File)
                .Include(c => c.InstitutionRltLabels).ThenInclude(d => d.Label)
                .Include(c => c.InstitutionRltEditions)
                );
            });
        }
    }
}