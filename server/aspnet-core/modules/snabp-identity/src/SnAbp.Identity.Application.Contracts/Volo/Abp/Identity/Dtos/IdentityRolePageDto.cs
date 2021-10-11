/**********************************************************************
*******命名空间： Volo.Abp.Identity.Dtos
*******类 名 称： IdentityRolePageDto
*******类 说 明： 为满足前端服务查询需求。新增角色分页实体
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/10 11:15:56
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Identity
{
    public class IdentityRolePageDto : PagedAndSortedResultRequestDto
    {
        public Guid? OrganizationId { get; set; }

        /// <summary>
        /// 是否包含功能角色（如果分配的时候包含，如果编辑的时候不能包含）
        /// </summary>
        public bool Public { get; set; } = false;

        /// <summary>
        /// 2021-2-5更改 是否为分配角色时查询
        /// </summary>
        public bool IsAssignRoles { get; set; } = false;
    }
}
