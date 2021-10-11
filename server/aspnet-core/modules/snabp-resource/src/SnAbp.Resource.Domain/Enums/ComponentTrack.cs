using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

/************************************************************************************
*命名空间：SnAbp.Resource.Enums
*文件名：ComponentTrack
*创建人： liushengtao
*创建时间：2021/6/24 10:03:37
*描述：
*
***********************************************************************/
namespace SnAbp.Resource.Enums
{
    public enum ActivatedState
    {
        [Description("已激活")]
        Activated = 1,

        [Description("未激活")]
        NoActivated = 2,
    }
    /// <summary>
    /// 检验、入库、出库、到场检验、安装、调试
    /// </summary>
    public enum NodeType
    {

        [Description("检验")]
        CheckOut = 1,

        [Description("入库")]
        PutStorage = 2,
        [Description("出库")]
        OutStorage = 3,

        [Description("到场检验")]
        ToTest = 4,
        [Description("安装")]
        Install = 5,

        [Description("调试")]
        Alignment = 6,
    }
}
