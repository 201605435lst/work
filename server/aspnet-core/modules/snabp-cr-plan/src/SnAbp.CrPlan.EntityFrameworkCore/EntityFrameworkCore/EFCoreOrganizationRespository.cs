using SnAbp.CrPlan.Entities;
using SnAbp.CrPlan.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.CrPlan.EntityFrameworkCore
{
    public class EFCoreOrganizationRespository : EfCoreRepository<ICrPlanDbContext, Organization, Guid>, IOrganizationRespository, ITransientDependency
    {
        public EFCoreOrganizationRespository(IDbContextProvider<ICrPlanDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
