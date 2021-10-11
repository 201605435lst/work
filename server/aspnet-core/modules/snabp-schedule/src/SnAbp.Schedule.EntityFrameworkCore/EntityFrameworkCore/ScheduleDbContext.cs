using Microsoft.EntityFrameworkCore;
using SnAbp.File.EntityFrameworkCore;
using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.Schedule.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Schedule.EntityFrameworkCore
{
    [ConnectionStringName(ScheduleDbProperties.ConnectionStringName)]
    public class ScheduleDbContext : AbpDbContext<ScheduleDbContext>, IScheduleDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public ScheduleDbContext(DbContextOptions<ScheduleDbContext> options)
            : base(options)
        {

        }

        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<ScheduleRltSchedule> ScheduleRltSchedule { get; set; }
        public DbSet<ScheduleFlowInfo> ScheduleFlowInfo { get; set; }
        public DbSet<ScheduleFlowTemplate> ScheduleFlowTemplate { get; set; }
        public DbSet<ScheduleRltProjectItem> ScheduleRltProjectItem { get; set; }
        public DbSet<ScheduleRltEquipment> ScheduleRltEquipment { get; set; }

        public DbSet<Approval> Approval { get; set; }
        public DbSet<ApprovalRltFile> ApprovalRltFile { get; set; }
        public DbSet<ApprovalRltMaterial> ApprovalRltMaterial { get; set; }
        public DbSet<ApprovalRltMember> ApprovalRltMember { get; set; }
        public DbSet<Diary> Diary { get; set; }
        public DbSet<DiaryRltFile> DiaryRltFile { get; set; }
        public DbSet<DiaryRltBuilder> DiaryRltBuilder { get; set; }
        public DbSet<DiaryRltMaterial> DiaryRltMaterial { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureSchedule();
            builder.ConfigureFile();
            builder.ConfigureIdentity();

        }
    }
}