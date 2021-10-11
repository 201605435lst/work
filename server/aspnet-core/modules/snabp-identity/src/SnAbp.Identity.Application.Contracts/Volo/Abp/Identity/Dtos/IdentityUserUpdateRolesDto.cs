using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SnAbp.Identity
{
    public class IdentityUserUpdateRolesDto
    {
        //[Required]
        /// public string[] RoleNames { get; set; }

        /// <summary>
        /// 角色id集合（Easten 新增）
        /// </summary>
        [Required]
        public List<Guid> RoleIds { get; set; }

        /// <summary>
        /// 组织机构Id，为空时为系统用户
        /// </summary>
        public Guid? OrganizationId { get; set; }

        //用户ids
        public List<Guid> UserIds{ get; set; }
    }
}