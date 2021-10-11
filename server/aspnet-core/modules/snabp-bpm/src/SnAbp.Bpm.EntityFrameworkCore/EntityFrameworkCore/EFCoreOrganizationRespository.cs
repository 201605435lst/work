using SnAbp.Bpm.Entities;
using SnAbp.Bpm.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Bpm.EntityFrameworkCore
{
    public class EFCoreOrganizationRespository : EfCoreRepository<IBpmDbContext, Organization, Guid>, IOrganizationRespository, ITransientDependency
    {
        public EFCoreOrganizationRespository(IDbContextProvider<IBpmDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
