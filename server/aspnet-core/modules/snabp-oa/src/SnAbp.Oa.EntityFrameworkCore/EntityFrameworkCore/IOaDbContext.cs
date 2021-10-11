using Microsoft.EntityFrameworkCore;

using SnAbp.Oa.Entities;

using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Oa.EntityFrameworkCore
{
    [ConnectionStringName(OaDbProperties.ConnectionStringName)]
    public interface IOaDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
       
        DbSet<DutySchedule> DutySchedule { get; }
        DbSet<DutyScheduleRltUser> DutyScheduleRltUser { get; }
        DbSet<Contract> Contract { get; }
        DbSet<ContractRltFile> ContractRltFile { get; }
        DbSet<Seal> Seal { get; }
        DbSet<SealRltMember> SealRltMember { get; }
    }
}