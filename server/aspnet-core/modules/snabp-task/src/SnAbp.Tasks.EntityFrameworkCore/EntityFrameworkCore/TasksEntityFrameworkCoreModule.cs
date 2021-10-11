using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.File.EntityFrameworkCore;
using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.Tasks.Entities;
using System;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Tasks.EntityFrameworkCore
{
    [DependsOn(
        typeof(TasksDomainModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(SnAbpIdentityEntityFrameworkCoreModule),
        typeof(FileEntityFrameworkCoreModule)
    )]
    public class TasksEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<TasksDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */

                options.AddRepository<Task, EfCoreRepository<ITasksDbContext, Task, Guid>>();
                options.AddRepository<TaskRltMember, EfCoreRepository<ITasksDbContext, TaskRltMember, Guid>>();
                options.AddRepository<TaskRltFile, EfCoreRepository<ITasksDbContext, TaskRltFile, Guid>>();

                options.Entity<Task>(x => x.DefaultWithDetailsFunc = q => q
                          .Include(x => x.TaskRltMembers).ThenInclude(y => y.Member)
                          .Include(x => x.TaskRltFiles).ThenInclude(y => y.File)
                          .Include(x => x.Project)
                 );

                options.Entity<TaskRltMember>(x => x.DefaultWithDetailsFunc = q => q
                          .Include(x => x.Task).ThenInclude(y => y.TaskRltMembers).ThenInclude(z => z.Member)
                 );

                options.Entity<TaskRltFile>(x => x.DefaultWithDetailsFunc = q => q
                          .Include(x => x.Task).ThenInclude(y => y.TaskRltFiles).ThenInclude(z => z.File)
                 );
            });
        }
    }
}