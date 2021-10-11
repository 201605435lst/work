using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using SnAbp.EntityFrameworkCore;
using SnAbp.Exam.Entities;
using Volo.Abp.Modularity;

namespace SnAbp.Exam.EntityFrameworkCore
{
    [DependsOn(
        typeof(ExamDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class ExamEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ExamDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddDefaultRepositories<IExamDbContext>(true);
                //options.AddRepository<Organization, EFCoreOrganizationRespository>();
                options.Entity<ExamPaperTemplate>(x => x.DefaultWithDetailsFunc = q => q
               .Include(x => x.ExamPaperTemplateConfigs)
               .Include(x => x.Category)
      );
                options.Entity<KnowledgePoint>(x => x.DefaultWithDetailsFunc = q => q
                        .Include(x => x.KnowledgePointRltCategories).ThenInclude(r => r.Category)
                    );
                //options.Entity<KnowledgePointRltCategory>(x => x.DefaultWithDetailsFunc = q => q
                //       .Include(x => x.Category)
                //       .Include(x => x.KnowledgePoint)
                //   );

                options.Entity<Question>(x => x.DefaultWithDetailsFunc = q => q
                        .Include(x => x.QuestionRltCategories).ThenInclude(r => r.Category)
                        .Include(x => x.QuestionRltKnowledgePoints).ThenInclude(r => r.KnowledgePoint)
                        .Include(x => x.AnswerOptions)
                    );
                options.Entity<QuestionRltKnowledgePoint>(x => x.DefaultWithDetailsFunc = q => q
                        .Include(x => x.Question)
                        .Include(x => x.KnowledgePoint)
                    );
                options.Entity<QuestionRltCategory>(x => x.DefaultWithDetailsFunc = q => q
                        .Include(x => x.Question)
                        .Include(x => x.Category)
                    );

                options.Entity<ExamPaper>(x => x.DefaultWithDetailsFunc = q => q
                        .Include(x => x.ExamPaperTemplate)
                        .Include(x => x.Category)
                );
            });

            //context.Services.AddScoped<IOrganizationRespository, EFCoreOrganizationRespository>();


        }
    }
}