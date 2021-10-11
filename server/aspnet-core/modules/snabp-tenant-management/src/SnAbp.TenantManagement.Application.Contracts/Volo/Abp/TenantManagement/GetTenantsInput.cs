using Volo.Abp.Application.Dtos;

namespace SnAbp.TenantManagement
{
    public class GetTenantsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}