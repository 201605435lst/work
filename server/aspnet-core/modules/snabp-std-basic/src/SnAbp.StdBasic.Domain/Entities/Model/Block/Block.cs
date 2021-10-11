using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 图块表
    /// </summary>
    public class Block : Entity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(50)]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 分类Id
        /// </summary>
        [ForeignKey("BlockCategory")]
        public Guid? BlockCategoryId { get; set; }
        public virtual BlockCategory BlockCategory { get; set; }

        /// <summary>
        /// 二维预览
        /// </summary>
        public string TwoDPreview { get; set; }

        /// <summary>
        /// 二维符号
        /// </summary>
        public string TwoDSymbol { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        protected Block() { }
        public Block(Guid id)
        {
            Id = id;
        }
    }
}
