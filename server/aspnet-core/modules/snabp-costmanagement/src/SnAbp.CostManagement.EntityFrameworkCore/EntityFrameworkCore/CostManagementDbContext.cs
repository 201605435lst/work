using Microsoft.EntityFrameworkCore;
using SnAbp.CostManagement.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.CostManagement.EntityFrameworkCore
{
    [ConnectionStringName(CostManagementDbProperties.ConnectionStringName)]
    public class CostManagementDbContext : AbpDbContext<CostManagementDbContext>, ICostManagementDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public CostManagementDbContext(DbContextOptions<CostManagementDbContext> options) 
            : base(options)
        {



        }
        public DbSet<PeopleCost> PeopleCost { get; set; }
        public DbSet<CostOther> CostOther { get; set; }
        public DbSet<MoneyList> MoneyList { get; set; }
        public DbSet<Contract> Contract { get; set; }
        public  DbSet<ContractRltFile> ContractRltFile { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureCostManagement();
        }
    }
}