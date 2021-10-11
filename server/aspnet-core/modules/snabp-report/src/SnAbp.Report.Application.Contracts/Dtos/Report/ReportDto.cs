

using SnAbp.Identity;
using SnAbp.Report.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Report.Dtos
{
    public class ReportDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual Guid ProjectId { get; set; }
        public virtual Project.Project Project { get; set; }
        // <summary>
        /// 所属组织机构
        /// </summary>
        public virtual Guid? OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        /// <summary>
        /// 汇报类型
        /// </summary>
        public ReportType Type { get; set; }
        /// <summary>
        /// 汇报日期
        /// </summary>Organization
        public DateTime Date { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 工作计划
        /// </summary>
        public string Plan { get; set; }
        /// <summary>
        /// 工作总结
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 工作记录
        /// </summary>
        public string WorkRecord { get; set; }
        /// <summary>
        /// 工作记录
        /// </summary>
        public string CreatorName { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<ReportRltFileDto> ReportRltFiles { get; set; } = new List<ReportRltFileDto>();
        /// <summary>
        /// 通知人员
        /// </summary>
        public List<ReportRltUserDto> ReportRltUsers { get; set; } = new List<ReportRltUserDto>();
    }
}
