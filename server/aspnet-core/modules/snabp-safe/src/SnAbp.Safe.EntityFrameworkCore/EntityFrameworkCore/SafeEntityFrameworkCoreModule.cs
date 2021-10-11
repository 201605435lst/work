using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.File.EntityFrameworkCore;
using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.Resource.EntityFrameworkCore;
using SnAbp.Safe.Entities;
using System;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Safe.EntityFrameworkCore
{
    [DependsOn(
        typeof(SafeDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class SafeEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<SafeDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */

                options.AddRepository<SafeProblemLibraryRltScop, EfCoreRepository<ISafeDbContext, SafeProblemLibraryRltScop, Guid>>();
                options.AddRepository<SafeProblemRecordRltFile, EfCoreRepository<ISafeDbContext, SafeProblemRecordRltFile, Guid>>();
                options.AddRepository<SafeProblemRltCcUser, EfCoreRepository<ISafeDbContext, SafeProblemRltCcUser, Guid>>();
                options.AddRepository<SafeProblemRltEquipment, EfCoreRepository<ISafeDbContext, SafeProblemRltEquipment, Guid>>();
                options.AddRepository<SafeProblem, EfCoreRepository<ISafeDbContext, SafeProblem, Guid>>();
                options.AddRepository<SafeProblemLibrary, EfCoreRepository<ISafeDbContext, SafeProblemLibrary, Guid>>();
                options.AddRepository<SafeProblemRecord, EfCoreRepository<ISafeDbContext, SafeProblemRecord, Guid>>();
                options.AddRepository<SafeSpeechVideo, EfCoreRepository<ISafeDbContext, SafeSpeechVideo, Guid>>();
                options.AddRepository<SafeProblemRltFile, EfCoreRepository<ISafeDbContext, SafeProblemRltFile, Guid>>();
                options.AddDefaultRepositories<ISafeDbContext>(true);
                //.AddDefaultRepositories<IIdentityDbContext>(true)
                //.AddDefaultRepositories<IFileDbContext>(true)
                //.AddDefaultRepositories<IResourceDbContext>(true);
                //options.AddRepository<SafeProblemRltCcUser, EfCoreRepository<SafeDbContext, SafeProblemRltCcUser, Guid>>();
                //options.AddRepository<SafeProblemRltEquipment, EfCoreRepository<SafeDbContext, SafeProblemRltEquipment, Guid>>();
                //options.AddRepository<SafeProblemRltFile, EfCoreRepository<SafeDbContext, SafeProblemRltFile, Guid>>();


                options.Entity<SafeProblem>(x => x.DefaultWithDetailsFunc = q => q
                                 .Include(x => x.Type)
                                 .Include(x => x.Checker)
                                 .Include(x => x.Profession)
                                 .Include(x => x.CheckUnit)
                                 .Include(x => x.ResponsibleUser)
                                 .Include(x => x.Verifier)
                                 .Include(x => x.ResponsibleOrganization)
                                 .Include(x => x.CcUsers).ThenInclude(y => y.CcUser)
                                 .Include(x => x.Files).ThenInclude(y => y.File)
                                 .Include(x => x.Equipments).ThenInclude(y => y.Equipment).ThenInclude(r => r.Group));
                options.Entity<SafeProblemLibrary>(x => x.DefaultWithDetailsFunc = q => q
                         .Include(x => x.EventType)
                         .Include(x => x.Profession)
                         .Include(x => x.Scops).ThenInclude(y => y.Scop));
                options.Entity<SafeProblemRecord>(x => x.DefaultWithDetailsFunc = q => q
                           .Include(x => x.SafeProblem)
                           .Include(x => x.User)
                           .Include(x => x.Files).ThenInclude(y => y.File));
            });
        }
    }
}