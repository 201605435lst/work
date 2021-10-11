using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.StdBasic.Enums
{
    /// <summary>
    /// 产品导入列标志
    /// </summary>
    public enum ProductImportCol
    {
        SeenSun,
        [Description("产品编码")]
        CSRGCode,
        [Description("产品分类")]
        IFDParent,
        [Description("产品名称")]
        IFD,
        [Description("产品型号")]
        Spec,
        [Description("单位")]
        Unit,
        [Description("厂家")]
        Manufacture,
    }
}
