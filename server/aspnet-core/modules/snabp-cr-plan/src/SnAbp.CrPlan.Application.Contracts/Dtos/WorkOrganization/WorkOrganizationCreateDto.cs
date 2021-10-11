using SnAbp.CrPlan.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto
{
    /// <summary>
    /// 派工单作业单位，添加修改数据使用
    /// </summary>
    public class WorkOrganizationCreateDto : EntityDto<Guid>, IRepairTagKeyDto
    {
        /// <summary>
        /// 派工单ID
        /// </summary>
        public Guid WorkOrderId { get; set; }

        /// <summary>
        /// 作业单位Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        public string RepairTagKey { get ; set ; }
    }
}
