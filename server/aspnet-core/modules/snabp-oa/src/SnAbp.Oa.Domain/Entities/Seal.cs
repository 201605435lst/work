/**********************************************************************
*******命名空间： SnAbp.Oa.Entities
*******类 名 称： Seal
*******类 说 明： 签章实体
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/1 11:57:14
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;

using SnAbp.Identity;

using Volo.Abp.Domain.Entities.Auditing;
using SnAbp.MultiProject.MultiProject;

namespace SnAbp.Oa.Entities
{
    public sealed class Seal : FullAuditedEntity<Guid>
    {
        public Seal(Guid id) => Id = id;
        /// <summary>
        /// 签章名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 签章类型
        /// </summary>
        public SealType Type { get; set; }
        /// <summary>
        /// 授权成员
        /// </summary>
        public List<SealRltMember> SealRltMembers { get; set; }

        /// <summary>
        /// 是否公开
        /// </summary>
        public bool IsPublic { get; set; }
        /// <summary>
        /// 签章密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 签名图片
        /// </summary>
        public Guid ImageId { get; set; }
        public File.Entities.File Image { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

    }
}
