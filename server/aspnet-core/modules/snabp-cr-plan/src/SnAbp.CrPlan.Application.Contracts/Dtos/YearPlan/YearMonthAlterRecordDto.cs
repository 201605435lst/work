using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dtos
{
    public class YearMonthAlterRecordDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 审批状态
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 计划类型
        /// </summary>
        public int PlanType { get; set; }

        public Guid? ARKey { get; set; }

        public string ChangeReason { get; set; }

        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; }



    }
}
