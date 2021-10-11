using SnAbp.Common.Eitities;
using SnAbp.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Common.EntityFrameworkCore
{
    public class EFCoreOrganizationRespository : EfCoreRepository<ICommonDbContext, Organization, Guid>, IOrganizationRespository, ITransientDependency
    {
        public EFCoreOrganizationRespository(IDbContextProvider<ICommonDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
