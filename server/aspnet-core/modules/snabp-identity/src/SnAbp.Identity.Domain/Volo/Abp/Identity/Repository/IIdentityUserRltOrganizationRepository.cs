using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Identity.Repository
{
   public interface IIdentityUserRltOrganizationRepository : IBasicRepository<IdentityUserRltOrganization>, IQueryable<IdentityUserRltOrganization>
    {
    }
}
