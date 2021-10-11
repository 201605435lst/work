using Microsoft.EntityFrameworkCore;
using SnAbp.Bpm.EntityFrameworkCore;
using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.Message.Bpm.Entities;

using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Message.Bpm.EntityFrameworkCore
{
    [ConnectionStringName(BpmDbProperties.ConnectionStringName)]
    public class BpmMessageDbContext : AbpDbContext<BpmMessageDbContext>, IBpmMessageDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public BpmMessageDbContext(DbContextOptions<BpmMessageDbContext> options) 
            : base(options)
        {

        }

        public DbSet<BpmRltMessage> BpmRltMessage { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureMessageBpm();
            builder.ConfigureIdentity();
            builder.ConfigureBpm();
        }
    }
}