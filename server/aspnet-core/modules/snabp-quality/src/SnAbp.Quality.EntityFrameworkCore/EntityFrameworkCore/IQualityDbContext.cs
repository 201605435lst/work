using Microsoft.EntityFrameworkCore;

using SnAbp.Quality.Entities;

using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Quality.EntityFrameworkCore
{
    [ConnectionStringName(QualityDbProperties.ConnectionStringName)]
    public interface IQualityDbContext : IEfCoreDbContext
    {
        DbSet<QualityProblemRecordRltFile> QualityProblemRecordRleFile { get; set; }
        DbSet<QualityProblem> QualityProblem { get; set; }
        DbSet<QualityProblemLibrary> QualityProblemLibrary { get; set; }
        DbSet<QualityProblemLibraryRltScop> QualityProblemLibraryRltScop { get; set; }
        DbSet<QualityProblemRecord> QualityProblemRecord { get; set; }
        DbSet<QualityProblemRltCcUser> QualityProblemRltCcUser { get; set; }
        DbSet<QualityProblemRltFile> QualityProblemRltFile { get; set; }
        DbSet<QualityProblemRltEquipment> QualityProblemRltEquipment { get; set; }
    }
}