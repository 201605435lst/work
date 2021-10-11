using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Tasks.EntityFrameworkCore
{
    public class TasksHttpApiHostMigrationsDbContext : AbpDbContext<TasksHttpApiHostMigrationsDbContext>
    {
        public TasksHttpApiHostMigrationsDbContext(DbContextOptions<TasksHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureTasks();
        }
    }
}
