using SnAbp.Construction.Dtos.Plan.Plan;
using SnAbp.Construction.Dtos.PlanMaterial;
using SnAbp.Construction.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

/************************************************************************************
*命名空间：SnAbp.Construction.Dtos.DailyRltPlan
*文件名：DailyRltPlanDto
*创建人： liushengtao
*创建时间：2021/7/23 11:43:36
*描述：
*
***********************************************************************/
namespace SnAbp.Construction.Dtos
{
   public class DailyRltPlanMaterialDto : EntityDto<Guid>
    {
        /// <summary>
        /// 施工日志
        /// </summary>
        public virtual Guid DailyId { get; set; }
        public virtual Daily Daily { get; set; }
        /// <summary>
        /// 施工计划
        /// </summary>
        public virtual Guid PlanMaterialId { get; set; }
        public virtual PlanMaterialDto PlanMaterial { get; set; }
        /// <summary>
        /// 当天完成量
        /// </summary>
        public int Count { get; set; }
    }
}
