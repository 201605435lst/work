using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Tasks.EntityFrameworkCore
{
    [ConnectionStringName(TasksDbProperties.ConnectionStringName)]
    public interface ITasksDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */

        DbSet<Task> Task { get; }
    }
}