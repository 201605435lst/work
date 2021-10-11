using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Identity
{
    public class GetIdentityUsersInput : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }

        public Guid? ExcludeOrganizationId { get; set; }
        public Guid? OrganizationId { get; set; }
        /// <summary>
        /// 是否完全加载
        /// </summary>
        public bool IsAll { get; set; }

        /// <summary>
        /// 是否查询全部组织机构
        /// </summary>
        public bool IsAllOrganization { get; set; }
    }
}