using System;

namespace SnAbp.Identity
{
    [Serializable]
    public class OrganizationEto
    {
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
    }
}