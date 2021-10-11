using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Identity
{
    public interface IOrganizationRltRoleRepository : IBasicRepository<OrganizationRltRole>, IQueryable<OrganizationRltRole>
    {

    }
}