using Microsoft.EntityFrameworkCore;
using SnAbp.Schedule.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Schedule.EntityFrameworkCore
{
    [ConnectionStringName(ScheduleDbProperties.ConnectionStringName)]
    public interface IScheduleDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
        DbSet<Schedule> Schedule { get; }
        DbSet<ScheduleRltSchedule> ScheduleRltSchedule { get; }
        DbSet<ScheduleFlowInfo> ScheduleFlowInfo { get; }
        DbSet<ScheduleFlowTemplate> ScheduleFlowTemplate { get; }
        DbSet<ScheduleRltProjectItem> ScheduleRltProjectItem { get; }
        DbSet<ScheduleRltEquipment> ScheduleRltEquipment { get; }


        DbSet<Approval> Approval { get; }
        DbSet<ApprovalRltFile> ApprovalRltFile { get; }
        DbSet<ApprovalRltMaterial> ApprovalRltMaterial { get; }
        DbSet<ApprovalRltMember> ApprovalRltMember { get; }
        DbSet<Diary> Diary { get; }
        DbSet<DiaryRltFile> DiaryRltFile { get; }
        DbSet<DiaryRltBuilder> DiaryRltBuilder { get; }
        DbSet<DiaryRltMaterial> DiaryRltMaterial { get; }
    }
}