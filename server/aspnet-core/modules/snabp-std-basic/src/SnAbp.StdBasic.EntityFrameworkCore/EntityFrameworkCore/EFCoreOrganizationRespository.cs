using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.StdBasic.EntityFrameworkCore
{
    public class EFCoreOrganizationRespository : EfCoreRepository<IStdBasicDbContext, Organization, Guid>, IOrganizationRespository, ITransientDependency
    {
        public EFCoreOrganizationRespository(IDbContextProvider<IStdBasicDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
