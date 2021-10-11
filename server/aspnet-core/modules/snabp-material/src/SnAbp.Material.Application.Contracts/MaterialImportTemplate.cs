using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Material
{
    /// <summary>
    /// 材料导入模板
    /// </summary>
    public class MaterialImportTemplate
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }  
        /// <summary>
        /// 材料名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public string Spec { get; set; }
        /// <summary>
        /// 材料单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 材料价格
        /// </summary>
        public decimal Price { get; set; }
    }
}
