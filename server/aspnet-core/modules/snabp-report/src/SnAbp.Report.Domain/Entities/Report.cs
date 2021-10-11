using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using SnAbp.Report.Entities;
using SnAbp.Report.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Report
{
    public class Report : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 项目名称
        /// </summary>
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        /// <summary>
        /// 所属组织机构
        /// </summary>
        public virtual Guid ? OrganizationId { get; set; }
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
        /// 附件
        /// </summary>
        public List<ReportRltFile> ReportRltFiles { get; set; }
        /// <summary>
        /// 通知人员
        /// </summary>
        public List<ReportRltUser> ReportRltUsers { get; set; }
        public Report() { }
        public Report(Guid id)
        {
            Id = id;
        }
        public void SetId(Guid id)
        {
            Id = id;
        }
    }
}


