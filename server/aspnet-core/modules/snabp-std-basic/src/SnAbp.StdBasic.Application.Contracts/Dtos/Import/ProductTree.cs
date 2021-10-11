using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic.Dtos
{
    /// <summary>
    /// 厂家导入树结构
    /// </summary>
    public class ProductTree
    {
        /// <summary>
        /// 节点ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 父级主键
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 详细资料
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 子集
        /// </summary>
        public List<ProductTree> Child { get; set; }
    }

    /// <summary>
    /// 产品导入列
    /// </summary>
    public class ProductImport
    {
        public int RowIndex { get; set; }

        /// <summary>
        /// 对应标准库产品id
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 产品分类
        /// </summary>
        public string IFDParent { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string IFD { get; set; }

        /// <summary>
        /// 产品型号
        /// </summary>
        public string Spec { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 厂商
        /// </summary>
        public string Manufacture { get; set; }

        public List<ProductItem> Items { get; set; }
    }

    public class ProductItem
    {
        public string Name { get; set; }

        public string ParentName { get; set; }

        public string Unit { get; set; }
    }
}
