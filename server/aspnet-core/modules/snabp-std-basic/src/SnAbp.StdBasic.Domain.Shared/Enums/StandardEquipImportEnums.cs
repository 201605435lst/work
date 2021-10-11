using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic.Enums
{
    /// <summary>
    /// 标准设备导入列标记
    /// </summary>
    public enum ImportCol
    {
        /// <summary>
        /// 序号
        /// </summary>
        SeenSun,
        /// <summary>
        /// 编码
        /// </summary>
        CSRGCode,
        /// <summary>
        /// 产品分类
        /// </summary>
        IFDParent,
        /// <summary>
        /// 产品名称
        /// </summary>
        IFD,
        /// <summary>
        /// 产品型号
        /// </summary>
        Spec,
        /// <summary>
        /// 计量单位
        /// </summary>
        Unit,
        /// <summary>
        /// 产品厂家
        /// </summary>
        Manufacture,
    }
}
