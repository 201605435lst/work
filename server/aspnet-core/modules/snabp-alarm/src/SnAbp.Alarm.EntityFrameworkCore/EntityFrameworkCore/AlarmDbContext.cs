using Microsoft.EntityFrameworkCore;
using SnAbp.Alarm.Entities;
using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.Resource.EntityFrameworkCore;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Alarm.EntityFrameworkCore
{
    [ConnectionStringName(AlarmDbProperties.ConnectionStringName)]
    public class AlarmDbContext : AbpDbContext<AlarmDbContext>, IAlarmDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */


        public DbSet<Entities.Alarm> Alarm { get; }
        public DbSet<AlarmConfig> AlarmConfig { get; }
        public DbSet<AlarmEquipmentIdBind> AlarmEquipmentIdBind { get; }

        public AlarmDbContext(DbContextOptions<AlarmDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureAlarm();
            builder.ConfigureResource();
            builder.ConfigureIdentity();
        }
    }
}