using SnAbp.Resource.Entities;
using SnAbp.Resource.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Resource.EntityFrameworkCore
{
    public class EFCoreOrganizationRespository : EfCoreRepository<IResourceDbContext, Organization, Guid>, IOrganizationRespository, ITransientDependency
    {
        public EFCoreOrganizationRespository(IDbContextProvider<IResourceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
