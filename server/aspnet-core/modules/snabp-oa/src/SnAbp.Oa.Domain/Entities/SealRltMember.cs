/**********************************************************************
*******命名空间： SnAbp.Oa.Entities
*******类 名 称： SealRltUser
*******类 说 明： 签章用户关联表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/1 13:46:54
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Oa.Entities
{
    public sealed class SealRltMember : Entity<Guid>
    {
        public SealRltMember(Guid id) => Id = id; 

        public Guid SealId { get; set; }
        public Seal Seal { get; set; }

        public Guid MemberId { get; set; }
        public MemberType MemberType { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

    }
}
