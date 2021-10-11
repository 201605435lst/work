/**********************************************************************
*******命名空间： Volo.Abp.Identity.Dtos
*******类 名 称： IdentityOrganizationGetDto
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/17 10:15:33
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Identity
{
    public class OrganizationGetListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 关键字：名称或者编码
        /// </summary>
        public string KeyWords { get; set; }


        /// <summary>
        /// 机构父级 id
        /// </summary>
        public Guid? ParentId { get; set; }


        /// <summary>
        /// 初始 Ids
        /// </summary>
        [CanBeNull] public List<Guid> Ids { get; set; }

        /// <summary>
        /// 是否完全加载
        /// </summary>
        public bool IsAll { get; set; }
        /// <summary>
        /// 是否为当前用户
        /// </summary>
        public bool IsCurrent { get; set; }
        /// <summary>
        /// 是否树加载
        /// </summary>
        public bool isTreeSearch { get; set; }
    }
}
