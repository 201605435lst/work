/**********************************************************************
*******命名空间： SnAbp.Resource.TempleteModel
*******类 名 称： EngineeringEquipmentModel
*******类 说 明： 工程设备模板实体
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/9 14:39:51
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Resource
{
    public class EngineeringEquipmentModel
    {
        public int Index { get; set; }
        /// <summary>
        /// 安装位置代号
        /// </summary>
        public string InstallationSiteName { get; set; }

        /// <summary>
        /// 设备分组
        /// </summary>
        public string EquipmentGroup { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 标准名称
        /// </summary>
        public string StandardName { get; set; }

        /// <summary>
        /// 构建编码
        /// </summary>
        public string ComponentCategory { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCategory { get; set; }
        /// <summary>
        /// 父级设备名称
        /// </summary>
        public string ParentName { get; set; }
        /// <summary>
        /// MVD属性（Json）
        /// </summary>
        public string MVDProperties { get; set; }
        /// <summary>
        /// 扩展属性
        /// </summary>
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    }
}
