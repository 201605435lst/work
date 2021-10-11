using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SnAbp.FileApprove.EntityFrameworkCore
{
    public class FileApproveHttpApiHostMigrationsDbContext : AbpDbContext<FileApproveHttpApiHostMigrationsDbContext>
    {
        public FileApproveHttpApiHostMigrationsDbContext(DbContextOptions<FileApproveHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureFileApprove();
        }
    }
}
