/**********************************************************************
*******命名空间： SnAbp.Resource.TempleteModel
*******类 名 称： CableWiringModel
*******类 说 明： 电缆配线实体
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/9 14:46:15
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Resource
{
    public class CableWiringModel
    {
        public  int Index { get; set; }

        /// <summary>
        /// 安装位置名称
        /// </summary>
        public  string InstallationSiteName { get; set; }
        /// <summary>
        /// 电缆名称
        /// </summary>
        public  string Name { get; set; }
        /// <summary>
        /// 电缆芯
        /// </summary>
        public  string CableCore { get; set; }
        /// <summary>
        /// 设备A安装位置代号
        /// </summary>
        public  string EquipmentAGroupName { get; set; }
        /// <summary>
        /// 设备A 名称
        /// </summary>
        public string EquipmentAName { get; set; }
        /// <summary>
        /// 设备A 端子名称
        /// </summary>
        public string EquipmentATerminalName { get; set; }


        public  string EquipmentBGroupName { get; set; }
       
        public  string EquipmentBName { get; set; }
       
        public  string EquipmentBTerminalName { get; set; }

    }
}
