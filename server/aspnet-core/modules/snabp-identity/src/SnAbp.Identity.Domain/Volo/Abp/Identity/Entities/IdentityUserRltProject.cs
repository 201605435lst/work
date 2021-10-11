/**********************************************************************
*******命名空间： Volo.Abp.Identity.Entities
*******类 名 称： IdentityUserRltProject
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 8/25/2021 2:12:33 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities;

namespace SnAbp.Identity
{
    /// <summary>
    ///  用户项目表
    /// </summary>
    public class IdentityUserRltProject:Entity<Guid>
    {
        protected IdentityUserRltProject()
        {

        }
        public IdentityUserRltProject(Guid userId,Guid projectId)
        {
            UserId = userId;
            ProjectTagId = projectId;
        }
        public Guid UserId { get; set; }
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { UserId, ProjectTagId };
        }
    }
}
