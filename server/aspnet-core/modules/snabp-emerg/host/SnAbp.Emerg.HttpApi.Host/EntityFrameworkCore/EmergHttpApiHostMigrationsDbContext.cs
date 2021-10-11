using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Emerg.EntityFrameworkCore
{
    public class EmergHttpApiHostMigrationsDbContext : AbpDbContext<EmergHttpApiHostMigrationsDbContext>
    {
        public EmergHttpApiHostMigrationsDbContext(DbContextOptions<EmergHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureEmerg();
        }
    }
}
