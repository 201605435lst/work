using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Exam.Entities
{
    /// <summary>
    /// 考卷模板
    /// </summary>
    public class ExamPaperTemplate : AuditedEntity<Guid>
    {
        protected ExamPaperTemplate() { }
        public ExamPaperTemplate(Guid id) { Id = id; }

        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(500)]
        public string Remark { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public virtual Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }

        /// <summary>
        /// 配置项
        /// </summary>
        public List<ExamPaperTemplateConfig> ExamPaperTemplateConfigs { get; set; }
    }
}
