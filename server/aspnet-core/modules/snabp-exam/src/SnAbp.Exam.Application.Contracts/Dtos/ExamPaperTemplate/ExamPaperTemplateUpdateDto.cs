using SnAbp.Exam.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Exam.Dtos
{
    public class ExamPaperTemplateUpdateDto : EntityDto<Guid>
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
        /// <summary>
        /// 配置项
        /// </summary>
        public List<ExamPaperTemplateConfigCreateDto> ExamPaperTemplateConfigs { get; set; } = new List<ExamPaperTemplateConfigCreateDto>();
    }
}