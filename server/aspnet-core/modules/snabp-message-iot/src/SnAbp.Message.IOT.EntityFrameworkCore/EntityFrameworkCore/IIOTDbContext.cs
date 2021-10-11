using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Message.IOT.EntityFrameworkCore
{
    [ConnectionStringName(IOTDbProperties.ConnectionStringName)]
    public interface IIOTDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
    }
}