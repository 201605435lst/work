using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.MultiProject
{
    public interface IOrganizationRoot
    {
        Guid? OrganizationRootId { get; set; }
        IDisposable Change(Guid? id);
    }
}
