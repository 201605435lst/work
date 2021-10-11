using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Project.Dtos
{
    public class ArchivesCategoryUpdateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 上级分类
        /// </summary>
        public virtual Guid? ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 是否加密
        /// </summary>
        public bool IsEncrypt { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
