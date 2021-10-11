using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.TenantManagement
{
    public class TenantDto : ExtensibleEntityDto<Guid>
    {
        public string Name { get; set; }
    }
}