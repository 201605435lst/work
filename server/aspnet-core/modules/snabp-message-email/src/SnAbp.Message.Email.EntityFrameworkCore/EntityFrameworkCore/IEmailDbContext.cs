using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Message.Email.EntityFrameworkCore
{
    [ConnectionStringName(EmailDbProperties.ConnectionStringName)]
    public interface IEmailDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
    }
}