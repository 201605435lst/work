using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SnAbp.Cms.Dtos
{
    public class CategoryCreateDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string Code { get; set; }

        /// <summary>
        /// 概要
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Summary { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 缩略图文件id
        /// </summary>
        [Required]
        public Guid ThumbId { get; set; }

        /// <summary>
        /// 使用启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200)]
        public string Remark { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public Guid? ParentId { get; set; }
    }
}
