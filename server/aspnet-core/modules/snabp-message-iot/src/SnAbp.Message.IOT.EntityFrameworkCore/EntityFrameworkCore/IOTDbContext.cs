using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Message.IOT.EntityFrameworkCore
{
    [ConnectionStringName(IOTDbProperties.ConnectionStringName)]
    public class IOTDbContext : AbpDbContext<IOTDbContext>, IIOTDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public IOTDbContext(DbContextOptions<IOTDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureIOT();
        }
    }
}