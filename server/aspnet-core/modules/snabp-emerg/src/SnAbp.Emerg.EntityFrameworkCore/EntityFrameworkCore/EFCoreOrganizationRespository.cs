using SnAbp.Emerg.Entities;
using SnAbp.Emerg.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Emerg.EntityFrameworkCore
{
    public class EFCoreOrganizationRespository : EfCoreRepository<IEmergDbContext, Organization, Guid>, IOrganizationRespository, ITransientDependency
    {
        public EFCoreOrganizationRespository(IDbContextProvider<IEmergDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
