using SnAbp.File.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Cms.Dtos
{
    public class CategoryDto: EntityDto<Guid>
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 概要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 缩略图文件id
        /// </summary>
        public Guid ThumbId { get; set; }

        /// <summary>
        /// 缩略图文件
        /// </summary>
        public virtual FileSimpleDto Thumb { get; set; }

        /// <summary>
        /// 使用启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }

        public List<CategoryDto> Children { get; set; } = new List<CategoryDto>();
    }
}
