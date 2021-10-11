using SnAbp.Resource.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Resource.Repositories
{
    public interface IOrganizationRespository: Volo.Abp.Domain.Repositories.IRepository<Organization,Guid>, ITransientDependency
    {
    }
}
