using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.File.EntityFrameworkCore;
using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.Oa.EntityFrameworkCore;
using SnAbp.Project.Entities;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Project.EntityFrameworkCore
{
    [DependsOn(
        typeof(ProjectDomainModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(SnAbpIdentityEntityFrameworkCoreModule),
        typeof(OaEntityFrameworkCoreModule),
        typeof(FileEntityFrameworkCoreModule)
    )]
    public class ProjectEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ProjectDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddDefaultRepositories<IProjectDbContext>(true);

                options.Entity<Project>(x => x.DefaultWithDetailsFunc = q => q
                          .Include(x => x.Organization)
                          .Include(x => x.Manager)
                          .Include(x => x.ProjectRltContracts).ThenInclude(x => x.Contract).ThenInclude(z => z.Type)
                          .Include(x => x.ProjectRltMembers).ThenInclude(y => y.Manager)
                          .Include(x => x.ProjectRltFiles).ThenInclude(y => y.File)
                          .Include(x => x.ProjectRltUnits).ThenInclude(y => y.Unit)
                          .Include(x => x.Type)
                 );
                options.Entity<Archives>(x => x.DefaultWithDetailsFunc = q => q
                          .Include(x => x.BooksClassification)
                          .Include(x => x.ArchivesCategory)
                  );
                options.Entity<Dossier>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(x => x.FileCategory)
                    .Include(x=>x.DossierRltFiles).ThenInclude(y => y.File)
            );
                //options.Entity<ProjectRltContract>(x => x.DefaultWithDetailsFunc = q =>q
                //    .Include(x=>x.Contract).ThenInclude(y=>y.Type)
                //);
            });
        }
    }
}