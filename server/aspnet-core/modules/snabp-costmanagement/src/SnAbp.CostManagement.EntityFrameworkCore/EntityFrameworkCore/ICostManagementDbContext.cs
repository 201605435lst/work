using Microsoft.EntityFrameworkCore;
using SnAbp.CostManagement.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.CostManagement.EntityFrameworkCore
{
    [ConnectionStringName(CostManagementDbProperties.ConnectionStringName)]
    public interface ICostManagementDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
        DbSet<PeopleCost> PeopleCost { get; }
        DbSet<CostOther> CostOther { get; }
        DbSet<MoneyList> MoneyList { get; }
        DbSet<Contract> Contract { get; }
        DbSet<ContractRltFile> ContractRltFile { get; }
    }
}