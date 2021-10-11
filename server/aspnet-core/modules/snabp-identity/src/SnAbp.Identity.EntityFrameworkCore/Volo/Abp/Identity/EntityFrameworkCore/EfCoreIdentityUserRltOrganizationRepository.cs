using SnAbp.Identity;
using SnAbp.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Identity.Repository;

namespace SnAbp.Identity.EntityFrameworkCore
{
   public class EfCoreIdentityUserRltOrganizationRepository : EfCoreRepository<IIdentityDbContext, IdentityUserRltOrganization>, IIdentityUserRltOrganizationRepository
    {
        public EfCoreIdentityUserRltOrganizationRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }
    }
}
