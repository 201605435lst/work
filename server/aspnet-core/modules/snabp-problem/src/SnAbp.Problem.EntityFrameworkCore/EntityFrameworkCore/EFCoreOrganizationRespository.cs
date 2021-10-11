using SnAbp.Problem.Entities;
using SnAbp.Problem.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Problem.EntityFrameworkCore
{
    public class EFCoreOrganizationRespository : EfCoreRepository<IProblemDbContext, Organization, Guid>, IOrganizationRespository, ITransientDependency
    {
        public EFCoreOrganizationRespository(IDbContextProvider<IProblemDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
