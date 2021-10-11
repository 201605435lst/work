using Microsoft.EntityFrameworkCore;
using SnAbp.Emerg.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Emerg.EntityFrameworkCore
{
    [ConnectionStringName(EmergDbProperties.ConnectionStringName)]
    public class EmergDbContext : AbpDbContext<EmergDbContext>, IEmergDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public DbSet<EmergPlan> EmergPlan { get; set; }
        public DbSet<EmergPlanRltComponentCategory> EmergPlanRltComponentCategory { get; set; }
        public DbSet<EmergPlanRltFile> EmergPlanRltFile { get; set; }
        public DbSet<EmergPlanRecordRltMember> EmergPlanRecordRltMember { get; set; }

        public DbSet<EmergPlanRecord> EmergPlanRecord { get; set; }
        public DbSet<EmergPlanProcessRecord> EmergPlanProcessRecord { get; set; }
        public DbSet<EmergPlanRecordRltComponentCategory> EmergPlanRecordRltComponentCategory { get; set; }
        public DbSet<EmergPlanRecordRltFile> EmergPlanRecordRltFile { get; set; }

        public DbSet<Fault> Fault { get; set; }
        public DbSet<FaultRltEquipment> FaultRltEquipment { get; set; }
        public DbSet<FaultRltComponentCategory> FaultRltComponentCategory { get; set; }

        public EmergDbContext(DbContextOptions<EmergDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureEmerg();
        }
    }
}