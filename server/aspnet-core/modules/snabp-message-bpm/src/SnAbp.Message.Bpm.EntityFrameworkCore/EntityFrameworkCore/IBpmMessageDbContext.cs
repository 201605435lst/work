using Microsoft.EntityFrameworkCore;
using SnAbp.Message.Bpm.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Message.Bpm.EntityFrameworkCore
{
    [ConnectionStringName(BpmDbProperties.ConnectionStringName)]
    public interface IBpmMessageDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */

        DbSet<BpmRltMessage> BpmRltMessage { get; }
    }
}