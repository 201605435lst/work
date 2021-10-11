using SnAbp.Construction.Plans;
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

/************************************************************************************
*命名空间：SnAbp.Construction.Entities.Daily
*文件名：DailyRltPlan
*创建人： liushengtao
*创建时间：2021/7/23 11:37:05
*描述：施工任务
*
***********************************************************************/
namespace SnAbp.Construction.Entities
{
   public class DailyRltPlanMaterial : Entity<Guid>
    {
        public DailyRltPlanMaterial(Guid id)
        {
            Id = id;
        }
        /// <summary>
        /// 施工日志
        /// </summary>
        public virtual Guid DailyId { get; set; }
        public virtual Daily Daily { get; set; }
        /// <summary>
        /// 施工计划
        /// </summary>
        public virtual Guid PlanMaterialId { get; set; }
        public virtual PlanMaterial PlanMaterial { get; set; }
        /// <summary>
        /// 当天完成量
        /// </summary>
        public int Count { get; set; }
    }
}
