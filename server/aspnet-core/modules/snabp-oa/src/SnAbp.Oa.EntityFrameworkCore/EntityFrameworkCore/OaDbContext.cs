using Microsoft.EntityFrameworkCore;
using SnAbp.Identity;
using SnAbp.Oa.Entities;

using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Oa.EntityFrameworkCore
{
    [ConnectionStringName(OaDbProperties.ConnectionStringName)]
    public class OaDbContext : AbpDbContext<OaDbContext>, IOaDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public OaDbContext(DbContextOptions<OaDbContext> options)
            : base(options)
        {

        }

        public DbSet<DutySchedule> DutySchedule { get; set; }


        public DbSet<DutyScheduleRltUser> DutyScheduleRltUser { get; set; }

        public DbSet<Contract> Contract { get; set; }

        public DbSet<ContractRltFile> ContractRltFile { get; set; }

        public DbSet<Seal> Seal { get; set; }

        public DbSet<SealRltMember> SealRltMember { get; set; }

      

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureOa();
        }
    }
}