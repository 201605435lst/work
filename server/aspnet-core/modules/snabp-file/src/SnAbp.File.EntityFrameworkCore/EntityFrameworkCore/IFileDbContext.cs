using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using SnAbp.File.Entities;
using Volo.Abp.Data;

namespace SnAbp.File.EntityFrameworkCore
{
    [ConnectionStringName(FileDbProperties.ConnectionStringName)]
    public interface IFileDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
        DbSet<Entities.File> File { get; set; }
        DbSet<FileRltPermissions> FileRltPermissions { get; set; }
        DbSet<FileRltShare> FileRltShare { get; set; }
        DbSet<FileRltTag> FileRltTag { get; set; }
        DbSet<FileVersion> FileVersion { get; set; }


        DbSet<Folder> Folder { get; set; }
        DbSet<FolderRltPermissions> FolderRltPermissions { get; set; }
        DbSet<FolderRltShare> FolderRltShare { get; set; }
        DbSet<FolderRltTag> FolderRltTag { get; set; }
        DbSet<Tag> Tag { get; set; }

        DbSet<OssServer> OssServer { get; set; }
    }
}