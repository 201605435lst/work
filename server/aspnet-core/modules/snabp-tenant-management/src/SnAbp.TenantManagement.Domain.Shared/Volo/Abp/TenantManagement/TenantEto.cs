using System;

namespace SnAbp.TenantManagement
{
    [Serializable]
    public class TenantEto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
