using Microsoft.EntityFrameworkCore;

using SnAbp.Safe.Entities;

using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Safe.EntityFrameworkCore
{
    [ConnectionStringName(SafeDbProperties.ConnectionStringName)]
    public interface ISafeDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */

        DbSet<SafeProblemRecordRltFile> SafeProblemRecordRleFile { get; set; }
        DbSet<SafeProblem> SafeProblem { get; set; }
        DbSet<SafeProblemLibrary> SafeProblemLibrary { get; set; }
        DbSet<SafeProblemLibraryRltScop> SafeProblemLibraryRltScop { get; set; }
        DbSet<SafeProblemRecord> SafeProblemRecord { get; set; }
        DbSet<SafeProblemRltCcUser> SafeProblemRltCcUser { get; set; }
        DbSet<SafeProblemRltFile> SafeProblemRltFile { get; set; }
        DbSet<SafeSpeechVideo> SafeSpeechVideo { get; set; }
        DbSet<SafeProblemRltEquipment> SafeProblemRltEquipment { get; set; }
    }
}