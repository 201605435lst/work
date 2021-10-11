using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Alarm.EntityFrameworkCore
{
    public class AlarmHttpApiHostMigrationsDbContext : AbpDbContext<AlarmHttpApiHostMigrationsDbContext>
    {
        public AlarmHttpApiHostMigrationsDbContext(DbContextOptions<AlarmHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureAlarm();
        }
    }
}
