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
    /// 定额分类Dto
    /// </summary>
    public class QuotaCategoryDto : EntityDto<Guid>, IGuidKeyTree<QuotaCategoryDto>
    {
        public QuotaCategoryDto Parent { get; set; }
        public List<QuotaCategoryDto> Children { get; set; } = new List<QuotaCategoryDto>();
        /// <summary>
        /// 上级单项工程id
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
        /// Specialty 来源于数据字典，止于专业节点（即每个专业的定额分类顶节点）
        /// </summary>
        public Guid SpecialtyId { get; set; }

        public string SpecialtyName { get; set; }

        /// <summary>
        ///标准编号 Code 来源于数据字典
        /// </summary>
        
        public Guid StandardCodeId { get; set; }

        /// <summary>
        ///标准编号 Code 来源于数据字典
        /// </summary>

        public string StandardCodeName { get; set; }

        /// <summary>
        /// 定额的内容
        /// </summary>
        public  string Content { get; set; }

        /// <summary>
        /// 行政区域
        /// </summary>
        public int AreaId { get; set; }
        /// <summary>
        /// 行政区划名称
        /// </summary>
        public string AreaName { get; set; }

    }
}
