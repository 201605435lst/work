/**********************************************************************
*******命名空间： SnAbp.Utils.ExcelHelper
*******类 名 称： ExcelFileType
*******类 说 明： 定义一个文件枚举，以便于满足多种需求
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/14 11:57:30
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Utils.ExcelHelper
{
    public enum ExcelFileType
    {
        /// <summary>
        /// 2007以上版本
        /// </summary>
        Xlsx,
        /// <summary>
        /// 2003以前版本
        /// </summary>
        Xls
    }
}
