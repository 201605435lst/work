/**********************************************************************
*******命名空间： SnAbp.Resource.TempleteModel
*******类 名 称： CabinetWiringModel
*******类 说 明： 机柜配线实体
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/9 14:50:00
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Resource
{
    public class CabinetWiringModel
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 安装位置代号
        /// </summary>
        public string InstallationSiteName { get; set; }
        /// <summary>
        /// 配线名称
        /// </summary>
        public string Name { get; set; }
    }
}
