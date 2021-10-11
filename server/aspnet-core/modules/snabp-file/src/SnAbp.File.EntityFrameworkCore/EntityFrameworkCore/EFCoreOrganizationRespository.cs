using SnAbp.File2.Entities;
using SnAbp.File2.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SnAbp.File2.EntityFrameworkCore
{
    public class EFCoreOrganizationRespository : EfCoreRepository<IFile2DbContext, Organization, Guid>, IOrganizationRespository, ITransientDependency
    {
        public EFCoreOrganizationRespository(IDbContextProvider<IFile2DbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
