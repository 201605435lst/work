using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Schedule.Entities;
using System;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Schedule.EntityFrameworkCore
{
    [DependsOn(
        typeof(ScheduleDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class ScheduleEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ScheduleDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddDefaultRepositories<IScheduleDbContext>(true);

                options.AddRepository<Schedule, EfCoreRepository<IScheduleDbContext, Schedule, Guid>>();
                options.AddRepository<ScheduleRltSchedule, EfCoreRepository<IScheduleDbContext, ScheduleRltSchedule, Guid>>();
                options.AddRepository<ScheduleFlowInfo, EfCoreRepository<IScheduleDbContext, ScheduleFlowInfo, Guid>>();
                options.AddRepository<ScheduleFlowTemplate, EfCoreRepository<IScheduleDbContext, ScheduleFlowTemplate, Guid>>();
                options.AddRepository<ScheduleRltProjectItem, EfCoreRepository<IScheduleDbContext, ScheduleRltProjectItem, Guid>>();
                options.AddRepository<Approval, EfCoreRepository<IScheduleDbContext, Approval, Guid>>();
                options.AddRepository<ApprovalRltFile, EfCoreRepository<IScheduleDbContext, ApprovalRltFile, Guid>>();
                options.AddRepository<ApprovalRltMaterial, EfCoreRepository<IScheduleDbContext, ApprovalRltMaterial, Guid>>();
                options.AddRepository<ApprovalRltMember, EfCoreRepository<IScheduleDbContext, ApprovalRltMember, Guid>>();
                options.AddRepository<ApprovalRltMember, EfCoreRepository<IScheduleDbContext, ApprovalRltMember, Guid>>();
                options.AddRepository<Diary, EfCoreRepository<IScheduleDbContext, Diary, Guid>>();
                options.AddRepository<DiaryRltBuilder, EfCoreRepository<IScheduleDbContext, DiaryRltBuilder, Guid>>();
                options.AddRepository<DiaryRltFile, EfCoreRepository<IScheduleDbContext, DiaryRltFile, Guid>>();
                options.AddRepository<DiaryRltMaterial, EfCoreRepository<IScheduleDbContext, DiaryRltMaterial, Guid>>();


                options.Entity<Schedule>(x => x.DefaultWithDetailsFunc = q => q
                          .Include(x => x.ScheduleRltSchedules)
                          .Include(x => x.ScheduleRltProjectItems)
                          .Include(x => x.Profession)
                          .Include(x => x.Children)
                );
                options.Entity<Approval>(x => x.DefaultWithDetailsFunc = q => q
                          .Include(x => x.Profession)
                          .Include(x => x.Schedule)
                          .Include(x => x.TemporaryEquipment)
                          .Include(x => x.SafetyCaution)
                          .Include(x => x.ApprovalRltMembers).ThenInclude(y => y.Member)
                          .Include(x => x.ApprovalRltMaterials)
                          .Include(x => x.ApprovalRltFiles).ThenInclude(y => y.File)
                );
                options.Entity<Diary>(x => x.DefaultWithDetailsFunc = q => q
                     .Include(p => p.DirectorsRltBuilders).ThenInclude(s => s.Builder)
                     .Include(x => x.DiaryRltMaterials)
                     .Include(x => x.Approval)
                     .Include(x=>x.DiaryRltFiles).ThenInclude(z=>z.File)
                   
           );
            });
        }
    }
}