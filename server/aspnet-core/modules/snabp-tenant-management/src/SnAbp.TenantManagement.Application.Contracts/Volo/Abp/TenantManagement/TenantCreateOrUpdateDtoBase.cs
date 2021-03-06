using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace SnAbp.TenantManagement
{
    public abstract class TenantCreateOrUpdateDtoBase : ExtensibleObject
    {
        [Required]
        [DynamicStringLength(typeof(TenantConsts), nameof(TenantConsts.MaxNameLength))]
        public string Name { get; set; }
    }
}