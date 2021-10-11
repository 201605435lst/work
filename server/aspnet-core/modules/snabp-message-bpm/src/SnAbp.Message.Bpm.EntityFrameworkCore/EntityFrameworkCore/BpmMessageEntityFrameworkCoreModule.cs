using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Bpm.EntityFrameworkCore;
using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.Message.Bpm.Entities;
using SnAbp.Message.Bpm.Repositorys;
using SnAbp.Message.Bpm.Services;

using Volo.Abp.Domain.Repositories;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Message.Bpm.EntityFrameworkCore
{
    [DependsOn(
        typeof(BpmDomainModule),
        typeof(SnAbpIdentityEntityFrameworkCoreModule),
        typeof(BpmEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class BpmMessageEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<BpmMessageDbContext>(options =>
            {
                options.AddDefaultRepositories<IBpmMessageDbContext>(true);
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddRepository<BpmRltMessage, EfCoreRepository<IBpmMessageDbContext, BpmRltMessage, Guid>>();
                options.Services.AddTransient<IBpmMessageRepository, BpmMessageRepository>();
                options.Entity<BpmRltMessage>(a => a.DefaultWithDetailsFunc = b =>
                        b.Include(c => c.Workflow)
                            .Include(c => c.Processor)
                            .Include(c => c.User)
                    );

            });

        }
    }
}