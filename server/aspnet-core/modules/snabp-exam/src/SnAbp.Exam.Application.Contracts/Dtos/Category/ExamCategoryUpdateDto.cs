using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Exam.Dtos
{
    public class ExamCategoryUpdateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        /// <summary>
        /// 分类描述
        /// </summary>
        [MaxLength(100)]
        public string Description { get; set; }
        /// <summary>
        /// 分类排序
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }
    }
}
