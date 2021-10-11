using SnAbp.Exam.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SnAbp.Exam.Dtos
{
    public class KnowledgePointCreateDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 分类 Id
        /// </summary>
        [Required]
        public List<Guid> CategoryIds { get; set; }


    }
}
