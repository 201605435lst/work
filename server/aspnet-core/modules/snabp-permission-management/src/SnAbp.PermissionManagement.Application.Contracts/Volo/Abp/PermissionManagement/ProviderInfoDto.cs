using System;

namespace SnAbp.PermissionManagement
{
    public class ProviderInfoDto
    {
        public string ProviderName { get; set; }

       // public string ProviderKey { get; set; }
        public Guid ProviderGuid { get; set; }
    }
}