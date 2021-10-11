using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Message.Notice.EntityFrameworkCore
{
    [ConnectionStringName(NoticeDbProperties.ConnectionStringName)]
    public interface INoticeDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */

        DbSet<Entities.Notice> Notice { get; }
    }
}