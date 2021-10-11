using Volo.Abp.Application.Dtos;

namespace SnAbp.Identity
{
    public class UserLookupSearchInputDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}