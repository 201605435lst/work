using SnAbp.Identity;
using SnAbp.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace Volo.Abp.Identity.EntityFrameworkCore
{
    public class EfCoreOrganizationRltRoleRepository : EfCoreRepository<IIdentityDbContext, OrganizationRltRole>, IOrganizationRltRoleRepository
    {
        public EfCoreOrganizationRltRoleRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }
    }
}
