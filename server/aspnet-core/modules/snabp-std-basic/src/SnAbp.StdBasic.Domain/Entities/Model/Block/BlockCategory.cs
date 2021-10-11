using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 图块分类表
    /// </summary>
    public class BlockCategory : Entity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(200)]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [MaxLength(50)]
        [Description("编码")]
        public string Code { get; set; }

        /// <summary>
        /// 上级编码
        /// </summary>
        public Guid? ParentId { get; set; }
        public virtual BlockCategory Parent { get; set; }
        public virtual List<BlockCategory> Children { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        protected BlockCategory() { }
        public BlockCategory(Guid id)
        {
            Id = id;
        }
    }
}