using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.StdBasic.Entities;
using System;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
//using SnAbp.StdBasic.Entities;
//using SnAbp.StdBasic.Repositories;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.StdBasic.EntityFrameworkCore
{
    [DependsOn(
        typeof(StdBasicDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class StdBasicEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<StdBasicDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddDefaultRepositories<IStdBasicDbContext>(true);
                options.Entity<ModelTerminal>(x => x.DefaultWithDetailsFunc = q => q
                        .Include(x => x.Model)
                        .Include(x => x.ProductCategory)
                    );

                options.Entity<Model>(x => x.DefaultWithDetailsFunc = q => q
                        .Include(x => x.Manufacturer)
                        .Include(x => x.ProductCategory).ThenInclude(s => s.Parent).ThenInclude(m => m.Parent)
                        .Include(x => x.ComponentCategory)
                        .Include(x => x.Terminals)
                 );

                options.Entity<ModelRltMVDProperty>(x => x.DefaultWithDetailsFunc = q => q
                      .Include(x => x.MVDProperty)
                 );
                options.Entity<ProductCategory>(x => x.DefaultWithDetailsFunc = q => q
                       .Include(x => x.Children)
                       .Include(x => x.Parent)
                       .ThenInclude(h => h.Parent)
                       .ThenInclude(r => r.Parent)
                       .ThenInclude(t => t.Parent)
                       .ThenInclude(y => y.Parent)
                       .ThenInclude(u => u.Parent)
                       .ThenInclude(f => f.Parent)
                );
                options.Entity<ProductCategoryRltMVDProperty>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(x => x.MVDProperty)
                );
                options.Entity<ComponentCategory>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(x => x.Parent)
                       .ThenInclude(h => h.Parent)
                       .ThenInclude(r => r.Parent)
                       .ThenInclude(t => t.Parent)
                       //.ThenInclude(y => y.Parent)
                       .ThenInclude(u => u.Parent)
                       .ThenInclude(f => f.Parent)
                    .Include(x => x.Children)
                    .Include(x => x.ComponentCategoryRltProductCategories).ThenInclude(y => y.ProductionCategory));
                options.Entity<ComponentCategoryRltMVDProperty>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(x => x.MVDProperty)
                );
                options.Entity<IndividualProject>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(x => x.Parent)
                    .Include(x => x.Children)
                    .Include(x => x.Specialty));

                options.Entity<ProcessTemplate>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(x => x.Parent)
                    .Include(x => x.Children));

                options.Entity<RepairItem>(x => x.DefaultWithDetailsFunc = q => q
                        .Include(x => x.Group).ThenInclude(s => s.Parent)
                        .Include(x => x.RepairTestItems).ThenInclude(s => s.File)
                        .Include(x => x.RepairItemRltComponentCategories).ThenInclude(s => s.ComponentCategory)
                        .Include(x => x.RepairItemRltOrganizationTypes).ThenInclude(y => y.OrganizationType));

                options.Entity<Manufacturer>(x => x.DefaultWithDetailsFunc = q => q
                       .Include(x => x.Children)
                       .Include(x => x.Parent)
                   );

                options.Entity<InfluenceRange>(x => x.DefaultWithDetailsFunc = q => q
                       .Include(x => x.Tag)
                   );
                options.Entity<WorkAttention>(x => x.DefaultWithDetailsFunc = q => q
                  .Include(x => x.Type)
                  );
                options.Entity<QuotaCategory>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(x => x.Area)
                    .Include(x => x.StandardCode)
                    .Include(x => x.Specialty)
                    .Include(x => x.Children)
                    .Include(x => x.Parent)
                    );

                options.Entity<BasePrice>(x => x.DefaultWithDetailsFunc = q => q
                       .Include(x => x.Area)
                       .Include(x => x.StandardCode)
                       .Include(x => x.ComputerCode)
                  );

                options.Entity<ModelFile>(x => x.DefaultWithDetailsFunc = q => q
                .Include(x => x.FamilyFile)
                .Include(x => x.Thumb)
                );

                //options.AddRepository<Organization, EFCoreOrganizationRespository>();
            });

            //context.Services.AddScoped<IOrganizationRespository, EFCoreOrganizationRespository>();


        }
    }
}