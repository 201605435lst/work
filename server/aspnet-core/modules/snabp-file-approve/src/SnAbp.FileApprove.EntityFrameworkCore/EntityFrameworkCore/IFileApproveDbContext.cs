using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using SnAbp.FileApprove.Entities;
using Volo.Abp.Data;

namespace SnAbp.FileApprove.EntityFrameworkCore
{
    [ConnectionStringName(FileApproveDbProperties.ConnectionStringName)]
    public interface IFileApproveDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
        DbSet<FileApprove> FileApprove { get; set; }
        DbSet<FileApproveRltFlow> FileApproveRltFlow { get; set; }
    }
}