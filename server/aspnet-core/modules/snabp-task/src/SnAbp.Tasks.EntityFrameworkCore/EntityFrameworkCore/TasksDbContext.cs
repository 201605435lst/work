using Microsoft.EntityFrameworkCore;
using SnAbp.File.EntityFrameworkCore;
using SnAbp.Identity.EntityFrameworkCore;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Tasks.EntityFrameworkCore
{
    [ConnectionStringName(TasksDbProperties.ConnectionStringName)]
    public class TasksDbContext : AbpDbContext<TasksDbContext>, ITasksDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public TasksDbContext(DbContextOptions<TasksDbContext> options) 
            : base(options)
        {

        }

        public DbSet<Task> Task { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            base.OnModelCreating(builder);

            builder.ConfigureTasks();
            builder.ConfigureIdentity();
            builder.ConfigureFile();
        }
    }
}