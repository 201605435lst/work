/**********************************************************************
*******命名空间： SnAbp.Construction.Entities.Daily
*******类 名 称： DailyRltSafe
*******类 说 明： 施工日志关联安全问题
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 7/16/2021 9:33:21 AM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.MultiProject.MultiProject;
using SnAbp.Safe.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Construction.Entities
{
    /// <summary>
    ///  施工日志关联安全问题
    /// </summary>
    public class DailyRltSafe : Entity<Guid>
    {
        public DailyRltSafe(Guid id)
        {
            Id = id;
        }
        /// <summary>
        /// 施工日志
        /// </summary>
        public virtual Guid DailyId { get; set; }
        public virtual Daily Daily { get; set; }
        /// <summary>
        /// 安全问题
        /// </summary>
        public virtual Guid SafeProblemId { get; set; }
        public virtual SafeProblem SafeProblem { get; set; }
    }
}