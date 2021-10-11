using Microsoft.EntityFrameworkCore;
using SnAbp.File.EntityFrameworkCore;
using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.Oa.EntityFrameworkCore;
using SnAbp.Project.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Project.EntityFrameworkCore
{
    [ConnectionStringName(ProjectDbProperties.ConnectionStringName)]
    public class ProjectDbContext : AbpDbContext<ProjectDbContext>, IProjectDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
            : base(options)
        {

        }

        public DbSet<Project> Project { get; set; }

        public DbSet<Unit> Unit { get; set; }

        public DbSet<ProjectRltUnit> ProjectRltUnits { get; set; }

        public DbSet<ProjectRltMember> ProjectRltMembers { get; set; }

        public DbSet<ProjectRltContract> ProjectRltContract { get; set; }
        public DbSet<ProjectRltFile> ProjectRltFile { get; set; }
        public DbSet<Archives> Archives { get; set; }

        public DbSet<ArchivesCategory> ArchivesCategory { get; set; }

        public DbSet<Dossier> Dossier { get; set; }

        public DbSet<DossierCategory> DossierCategory { get; set; }
        public DbSet<DossierRltFile> DossierRltFile { get; set; }

        public DbSet<FileCategory> FileCategory { get; set; }
        public DbSet<BooksClassification> BooksClassification { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureProject();
            builder.ConfigureIdentity();
            builder.ConfigureOa();
            builder.ConfigureFile();
        }
    }
}