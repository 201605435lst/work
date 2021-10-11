using Microsoft.EntityFrameworkCore;
using SnAbp.File.EntityFrameworkCore;
using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.Quality.Entities;
using SnAbp.Resource.EntityFrameworkCore;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Quality.EntityFrameworkCore
{
    [ConnectionStringName(QualityDbProperties.ConnectionStringName)]
    public class QualityDbContext : AbpDbContext<QualityDbContext>, IQualityDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public QualityDbContext(DbContextOptions<QualityDbContext> options) 
            : base(options)
        {

        }
        public DbSet<QualityProblemRecordRltFile> QualityProblemRecordRleFile { get; set; }
        public DbSet<QualityProblem> QualityProblem { get; set; }
        public DbSet<QualityProblemLibrary> QualityProblemLibrary { get; set; }
        public DbSet<QualityProblemLibraryRltScop> QualityProblemLibraryRltScop { get; set; }
        public DbSet<QualityProblemRecord> QualityProblemRecord { get; set; }
        public DbSet<QualityProblemRltCcUser> QualityProblemRltCcUser { get; set; }
        public DbSet<QualityProblemRltFile> QualityProblemRltFile { get; set; }
        public DbSet<QualityProblemRltEquipment> QualityProblemRltEquipment { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //// 添加依赖
            builder.ConfigureFile();
            builder.ConfigureIdentity();
            builder.ConfigureResource();
            builder.ConfigureQuality();
        }
    }
}