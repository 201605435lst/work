using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dtos
{
    public class WorkTicketDeleteDto
    {
        /// <summary>
        /// 关联的垂直天窗的id
        /// </summary>
        //public Guid SkylightPlanId { get; set; }

        /// <summary>
        /// 工作票Id
        /// </summary>
        public Guid  TicketId { get; set; }
    }
}
