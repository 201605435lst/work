using Microsoft.EntityFrameworkCore;

using SnAbp.File.EntityFrameworkCore;
using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.Resource.EntityFrameworkCore;
using SnAbp.Safe.Entities;

using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Safe.EntityFrameworkCore
{
    [ConnectionStringName(SafeDbProperties.ConnectionStringName)]
    public class SafeDbContext : AbpDbContext<SafeDbContext>, ISafeDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public SafeDbContext(DbContextOptions<SafeDbContext> options)
            : base(options)
        {

        }
        public DbSet<SafeProblemRecordRltFile> SafeProblemRecordRleFile { get; set; }
        public DbSet<SafeProblem> SafeProblem { get; set; }
        public DbSet<SafeProblemLibrary> SafeProblemLibrary { get; set; }
        public DbSet<SafeProblemLibraryRltScop> SafeProblemLibraryRltScop { get; set; }
        public DbSet<SafeProblemRecord> SafeProblemRecord { get; set; }
        public DbSet<SafeProblemRltCcUser> SafeProblemRltCcUser { get; set; }
        public DbSet<SafeProblemRltFile> SafeProblemRltFile { get; set; }
        public DbSet<SafeSpeechVideo> SafeSpeechVideo { get; set; }
        public DbSet<SafeProblemRltEquipment> SafeProblemRltEquipment { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureSafe();
            //// 添加依赖
            builder.ConfigureFile();
            builder.ConfigureIdentity();
            builder.ConfigureResource();
        }
    }
}