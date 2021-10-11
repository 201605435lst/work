using SnAbp.Exam.Entities;
using SnAbp.Exam.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SnAbp.Exam.EntityFrameworkCore
{
    public class EFCoreOrganizationRespository : EfCoreRepository<IExamDbContext, Organization, Guid>, IOrganizationRespository, ITransientDependency
    {
        public EFCoreOrganizationRespository(IDbContextProvider<IExamDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
