using SnAbp.Exam.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Exam.Dtos
{
    public class ExamPaperTemplateDto :AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public virtual Guid CategoryId { get; set; }

        public virtual Category Category { get; set; }

        /// <summary>
        /// 配置项
        /// </summary>
        public List<ExamPaperTemplateConfigDto> ExamPaperTemplateConfigs { get; set; } = new List<ExamPaperTemplateConfigDto>();
        
    }
}
