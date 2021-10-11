using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace SnAbp.MultiProject
{
    public class OrganizationRoot : IOrganizationRoot, ITransientDependency
    {
        public Guid? OrganizationRootId { get; set; }

        public IDisposable Change(Guid? id)
        {
            return new DisposeAction(() => OrganizationRootId = id);
        }
    }
}
