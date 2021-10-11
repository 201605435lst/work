using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Bpm.Entities
{
    [NotMapped]
    /// <summary>
    /// 工作流简报
    /// </summary>
    public class WorkflowInfo
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        /// <summary>
        /// 表单索引
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 简报
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
    }
}
