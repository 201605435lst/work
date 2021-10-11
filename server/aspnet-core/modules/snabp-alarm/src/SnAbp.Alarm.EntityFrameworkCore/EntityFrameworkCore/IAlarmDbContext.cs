using Microsoft.EntityFrameworkCore;
using SnAbp.Alarm.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Alarm.EntityFrameworkCore
{
    [ConnectionStringName(AlarmDbProperties.ConnectionStringName)]
    public interface IAlarmDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */


        DbSet<Entities.Alarm> Alarm { get; }
        DbSet<AlarmConfig> AlarmConfig { get; }
        DbSet<AlarmEquipmentIdBind> AlarmEquipmentIdBind { get; }
    }
}