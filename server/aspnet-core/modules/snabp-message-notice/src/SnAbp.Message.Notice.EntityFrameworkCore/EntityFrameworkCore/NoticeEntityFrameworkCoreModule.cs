using System;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Message.Notice.IRepositorys;
using SnAbp.Message.Notice.Repositorys;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Message.Notice.EntityFrameworkCore
{
    [DependsOn(
        typeof(NoticeDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class NoticeEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<NoticeDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.Services.AddTransient<INoticeMessageRepository, NoticeMessageRepository>();
                options.AddRepository<Entities.Notice, EfCoreRepository<INoticeDbContext, Entities.Notice, Guid>>();
            });
        }
    }
}