using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Exam.Dtos
{
    public class ExamCategoryDto : EntityDto<Guid>
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 分类描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 分类排序
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid ? ParentId { get; set; }
        public List<ExamCategoryDto> Children { get; set; } = new List<ExamCategoryDto>();
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime ? CreationTime { get; set; }
    }
}
