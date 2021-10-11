using Microsoft.EntityFrameworkCore;
using SnAbp.Emerg.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Emerg.EntityFrameworkCore
{
    [ConnectionStringName(EmergDbProperties.ConnectionStringName)]
    public interface IEmergDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */

        DbSet<EmergPlan> EmergPlan { get; set; }
        DbSet<EmergPlanRltComponentCategory> EmergPlanRltComponentCategory { get; set; }
        DbSet<EmergPlanRltFile> EmergPlanRltFile { get; set; }
        DbSet<EmergPlanRecordRltMember> EmergPlanRecordRltMember { get; set; }

        DbSet<EmergPlanRecord> EmergPlanRecord { get; set; }
        DbSet<EmergPlanProcessRecord> EmergPlanProcessRecord { get; set; }
        
        DbSet<EmergPlanRecordRltComponentCategory> EmergPlanRecordRltComponentCategory { get; set; }
        DbSet<EmergPlanRecordRltFile> EmergPlanRecordRltFile { get; set; }

        DbSet<Fault> Fault { get; set; }
        DbSet<FaultRltEquipment> FaultRltEquipment { get; set; }
        DbSet<FaultRltComponentCategory> FaultRltComponentCategory { get; set; }


    }
}