using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic.Dtos
{
    /// <summary>
    /// 定额分类Dto
    /// </summary>
    public class QuotaCategoryCreateDto
    {
        public  Guid? ParentId { get; set; }
        /// <summary>
        /// 定额分类中的章节名称
        /// </summary>
        public  string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 专业 来源于数据字典，止于专业节点（即每个专业的定额分类顶节点）
        /// </summary>
        public Guid SpecialtyId { get; set; }

        /// <summary>
        ///标准编号 Code 来源于数据字典
        /// </summary>
        public Guid StandardCodeId { get; set; }

        /// <summary>
        /// 定额的内容
        /// </summary>
        public  string Content { get; set; }
        /// <summary>
        /// 行政区域
        /// </summary>
        public  int AreaId { get; set; }
    }
}
