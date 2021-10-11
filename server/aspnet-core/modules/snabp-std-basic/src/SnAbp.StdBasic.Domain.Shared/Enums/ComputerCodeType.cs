/**********************************************************************
*******命名空间： SnAbp.StdBasic.Enums
*******类 名 称： ComputerCodeType
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/29 10:01:10
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.StdBasic.Enums
{
    /// <summary>
    /// $$
    /// </summary>
    public enum ComputerCodeType
    {

        /// <summary>
        /// 人工
        /// </summary>
        [Description("人工")]
        Artificial=1,
        /// <summary>
        /// 机械
        /// </summary>
        [Description("机械")]
        Mechanics=2,
        /// <summary>
        /// 材料
        /// </summary>
        [Description("材料")]
        Material=3
    }
}
