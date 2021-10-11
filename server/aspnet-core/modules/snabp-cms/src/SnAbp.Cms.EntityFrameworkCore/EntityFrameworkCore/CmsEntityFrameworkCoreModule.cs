using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Cms.Entities;
//using SnAbp.Cms.Repositories;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Cms.EntityFrameworkCore
{
    [DependsOn(
        typeof(CmsDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class CmsEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<CmsDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddDefaultRepositories<ICmsDbContext>(true);

                //options.Entity<Category>(x => x.DefaultWithDetailsFunc = q => q
                //        .Include(x => x.Thumb)
                //        .Include(x => x.Children).ThenInclude(s => s.Children).ThenInclude(m => m.Children)
                //    );

                options.Entity<Article>(x => x.DefaultWithDetailsFunc = q => q
                        .Include(x => x.Accessories).ThenInclude(r => r.File)
                        .Include(x => x.Carousels).ThenInclude(r => r.File)
                        .Include(x => x.Categories).ThenInclude(r => r.Category)
                        .Include(x => x.Thumb)
                    );

                options.Entity<CategoryRltArticle>(x => x.DefaultWithDetailsFunc = q => q
                        .Include(x => x.Article)
                        .Include(x => x.Category)
                    );

                //options.AddRepository<Organization, EFCoreOrganizationRespository>();
            });

            //context.Services.AddScoped<IOrganizationRespository, EFCoreOrganizationRespository>();


        }
    }
}