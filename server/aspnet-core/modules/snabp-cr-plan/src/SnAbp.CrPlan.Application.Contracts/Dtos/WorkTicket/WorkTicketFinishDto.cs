using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dtos
{
   public class WorkTicketFinishDto : EntityDto<Guid>
    {
        /// <summary>
        /// 命令号
        /// </summary>
        public string OrderNumber { get; set; }


        /// <summary>
        /// 影响范围
        /// </summary>
        public string InfluenceRange { get; set; }


        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? RealStartTime { get; set; }

        /// <summary>
        /// 实际结束时间
        /// </summary>
        public DateTime? RealFinsihTime { get; set; }


        /// <summary>
        /// 工作完成情况
        /// </summary>
        public string FinishContent { get; set; }

        public bool?  IsFine { get; set; }
    }
}
