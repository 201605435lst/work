using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using SnAbp.File.Entities;
using Volo.Abp.Data;

namespace SnAbp.File.EntityFrameworkCore
{
    [ConnectionStringName(FileDbProperties.ConnectionStringName)]
    public class FileDbContext : AbpDbContext<FileDbContext>, IFileDbContext
    {
        public FileDbContext(DbContextOptions<FileDbContext> options)
            : base(options)
        {
        }
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */


        public DbSet<Entities.File> File { get; set; }
        public DbSet<Folder> Folder { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<FileVersion> FileVersion { get; set; }
        public DbSet<FileRltPermissions> FileRltPermissions { get; set; }
        public DbSet<FileRltShare> FileRltShare { get; set; }
        public DbSet<FileRltTag> FileRltTag { get; set; }
        public DbSet<FolderRltPermissions> FolderRltPermissions { get; set; }
        public DbSet<FolderRltShare> FolderRltShare { get; set; }
        public DbSet<FolderRltTag> FolderRltTag { get; set; }
        public DbSet<OssServer> OssServer { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureFile();
        }
    }
}