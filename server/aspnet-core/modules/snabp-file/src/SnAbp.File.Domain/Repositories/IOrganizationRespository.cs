using SnAbp.File2.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.File2.Repositories
{
    public interface IOrganizationRespository: Volo.Abp.Domain.Repositories.IRepository<Organization,Guid>, ITransientDependency
    {
    }
}
