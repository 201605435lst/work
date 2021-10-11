using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    /// <summary>
    /// 定额分类树Dto
    /// </summary>
    public class QuotaCategoryTreeDto : EntityDto<Guid>, IGuidKeyTree<QuotaCategoryTreeDto>
    {
        public QuotaCategoryTreeDto Parent { get; set; }
        public List<QuotaCategoryTreeDto> Children { get; set; } = new List<QuotaCategoryTreeDto>();
        /// <summary>
        /// 上级id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 定额分类中的章节名称
        /// </summary>
        [MaxLength(100)]
        [Description("名称")]
        public  string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [MaxLength(50)]
        [Description("编码")]
        public string Code { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

        public string QuotaCategoryName { get; set; }
        /// <summary>
        /// 工作内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 计量单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public float Weight { get; set; }

    }
}
