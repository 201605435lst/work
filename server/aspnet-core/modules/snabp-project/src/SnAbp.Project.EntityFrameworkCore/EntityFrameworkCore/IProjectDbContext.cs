using Microsoft.EntityFrameworkCore;
using SnAbp.Project.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Project.EntityFrameworkCore
{
    [ConnectionStringName(ProjectDbProperties.ConnectionStringName)]
    public interface IProjectDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */

        DbSet<Project> Project { get; }

        DbSet<Unit> Unit { get; }

        DbSet<ProjectRltUnit> ProjectRltUnits { get; }

        DbSet<ProjectRltMember> ProjectRltMembers { get; }

        DbSet<ProjectRltContract> ProjectRltContract { get; }

        DbSet<ProjectRltFile> ProjectRltFile { get; }
        DbSet<Archives> Archives { get; }

        DbSet<ArchivesCategory> ArchivesCategory { get; }

        DbSet<Dossier> Dossier { get; }

        DbSet<DossierCategory> DossierCategory { get; }
        DbSet<DossierRltFile> DossierRltFile { get; }

        DbSet<FileCategory> FileCategory { get; }
        DbSet<BooksClassification> BooksClassification { get; }

    }
}