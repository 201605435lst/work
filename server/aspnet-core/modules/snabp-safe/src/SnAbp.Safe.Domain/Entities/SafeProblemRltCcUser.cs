/**********************************************************************
*******命名空间： SnAbp.Safe.Entities
*******类 名 称： SafeProblemRltCcUser
*******类 说 明： 抄送关联表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/4/29 18:58:14
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities;

namespace SnAbp.Safe.Entities
{
    /// <summary>
    /// 抄送关联表
    /// </summary>
    public class SafeProblemRltCcUser : Entity<Guid>
    {
        public Guid SafeProblemId { get; set; }
        public SafeProblem SafeProblem { get; set; }
        public Guid CcUserId { get; set; }
        public virtual Identity.IdentityUser CcUser { get; set; }
        //public override object[] GetKeys()
        //{
        //    return new object[] { SafeProblemId, CcUserId };
        //}
        public SafeProblemRltCcUser(Guid id)
        {
            Id = id;
        }
    }
}
