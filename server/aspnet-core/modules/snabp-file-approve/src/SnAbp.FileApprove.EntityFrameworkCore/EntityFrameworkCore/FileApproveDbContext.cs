using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using SnAbp.FileApprove.Entities;
using Volo.Abp.Data;

namespace SnAbp.FileApprove.EntityFrameworkCore
{
    [ConnectionStringName(FileApproveDbProperties.ConnectionStringName)]
    public class FileApproveDbContext : AbpDbContext<FileApproveDbContext>, IFileApproveDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */
        public DbSet<FileApprove> FileApprove { get; set; }
        public DbSet<FileApproveRltFlow> FileApproveRltFlow { get; set; }
        public FileApproveDbContext(DbContextOptions<FileApproveDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureFileApprove();
        }
    }
}