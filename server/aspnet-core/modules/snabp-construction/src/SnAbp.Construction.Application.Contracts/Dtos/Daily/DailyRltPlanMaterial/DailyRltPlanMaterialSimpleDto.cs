using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

/************************************************************************************
*命名空间：SnAbp.Construction.Dtos
*文件名：DailyRltPlanCreateDto
*创建人： liushengtao
*创建时间：2021/7/23 11:43:51
*描述：
*
***********************************************************************/
namespace SnAbp.Construction.Dtos
{
   public class DailyRltPlanMaterialSimpleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 施工日志
        /// </summary>
        public virtual Guid DailyId { get; set; }
        /// <summary>
        /// 施工计划
        /// </summary>
        public virtual Guid PlanMaterialId { get; set; }
        /// <summary>
        /// 当天完成量
        /// </summary>
        public int Count { get; set; }
    }
}
