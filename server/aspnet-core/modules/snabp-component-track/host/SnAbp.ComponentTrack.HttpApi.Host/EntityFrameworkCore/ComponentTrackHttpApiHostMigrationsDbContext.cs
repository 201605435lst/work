using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.ComponentTrack.EntityFrameworkCore
{
    public class ComponentTrackHttpApiHostMigrationsDbContext : AbpDbContext<ComponentTrackHttpApiHostMigrationsDbContext>
    {
        public ComponentTrackHttpApiHostMigrationsDbContext(DbContextOptions<ComponentTrackHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureComponentTrack();
        }
    }
}
