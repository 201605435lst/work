using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Schedule.EntityFrameworkCore
{
    public class ScheduleHttpApiHostMigrationsDbContext : AbpDbContext<ScheduleHttpApiHostMigrationsDbContext>
    {
        public ScheduleHttpApiHostMigrationsDbContext(DbContextOptions<ScheduleHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureSchedule();
        }
    }
}
