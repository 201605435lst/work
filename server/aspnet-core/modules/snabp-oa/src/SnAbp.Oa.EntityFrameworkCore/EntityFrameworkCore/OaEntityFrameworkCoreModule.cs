using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Oa.Entities;
using System;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Oa.EntityFrameworkCore
{
    [DependsOn(
        typeof(OaDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class OaEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<OaDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * 
                 */
                options.AddRepository<DutyScheduleRltUser, EfCoreRepository<IOaDbContext, DutyScheduleRltUser, Guid>>();
                options.AddRepository<DutySchedule, EfCoreRepository<IOaDbContext, DutySchedule, Guid>>();

                options.AddRepository<Contract, EfCoreRepository<IOaDbContext, Contract, Guid>>();
                options.AddRepository<ContractRltFile, EfCoreRepository<IOaDbContext, ContractRltFile, Guid>>();
               

                options.AddRepository<Seal, EfCoreRepository<IOaDbContext, Seal, Guid>>();
                options.AddRepository<SealRltMember, EfCoreRepository<IOaDbContext, SealRltMember, Guid>>();


                options.Entity<DutySchedule>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(x => x.DutyScheduleRltUsers).ThenInclude(y => y.User).ThenInclude(p => p.Position)
                );
                options.Entity<Contract>(x => x.DefaultWithDetailsFunc = q => q
                   .Include(x => x.UnderDepartment)
                   .Include(x => x.Undertaker)
                   .Include(x => x.HostDepartment)
                   .Include(x => x.ContractRltFiles).ThenInclude(y=>y.File)
                   .Include(x => x.Type)
                );
                options.Entity<Seal>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(x => x.SealRltMembers)
                    .Include(x => x.Image)
                );
            });
        }
    }
}