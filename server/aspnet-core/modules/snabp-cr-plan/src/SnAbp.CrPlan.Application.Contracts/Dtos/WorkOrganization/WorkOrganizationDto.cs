using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.WorkOrganization
{
    /// <summary>
    /// 派工单作业单位，获取数据使用
    /// </summary>
    public class WorkOrganizationDto : EntityDto<Guid>, IRepairTagDto
    {
        /// <summary>
        /// 派工单ID
        /// </summary>
        public Guid WorkOrderId { get; set; }

        /// <summary>
        /// 作业单位Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 作业单位名称
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// 类型
        /// 0-检修，1-验收
        /// </summary>
        public Duty Duty { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }

        public WorkOrganizationDto() { }
        public WorkOrganizationDto(Guid id) { Id = id; }
    }
}
