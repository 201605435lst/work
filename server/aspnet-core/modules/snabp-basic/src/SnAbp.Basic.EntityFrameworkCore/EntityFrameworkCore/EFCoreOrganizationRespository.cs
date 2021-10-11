using SnAbp.Basic.Entities;
using SnAbp.Basic.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Basic.EntityFrameworkCore
{
    public class EFCoreOrganizationRespository : EfCoreRepository<IBasicDbContext, Organization, Guid>, IOrganizationRespository, ITransientDependency
    {
        public EFCoreOrganizationRespository(IDbContextProvider<IBasicDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
