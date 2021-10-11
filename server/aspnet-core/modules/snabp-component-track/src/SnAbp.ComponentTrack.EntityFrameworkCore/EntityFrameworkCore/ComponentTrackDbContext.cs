using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.ComponentTrack.EntityFrameworkCore
{
    [ConnectionStringName(ComponentTrackDbProperties.ConnectionStringName)]
    public class ComponentTrackDbContext : AbpDbContext<ComponentTrackDbContext>, IComponentTrackDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */
       

        public ComponentTrackDbContext(DbContextOptions<ComponentTrackDbContext> options)
            : base(options)
        {

        }
      
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureComponentTrack();
        }
    }
}