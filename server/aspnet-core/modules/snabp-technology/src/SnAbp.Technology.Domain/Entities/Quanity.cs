/**********************************************************************
*******命名空间： SnAbp.Technology.Entities
*******类 名 称： Quanity
*******类 说 明： 工程量实体
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/23/2021 4:00:29 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Technology.Entities
{
    /// <summary>
    ///  工程量实体
    /// </summary>
    public class Quanity
    {
        /// <summary>
        /// 专业
        /// </summary>
        public string Speciality { get; set; }
        /// <summary>
        /// 系统1
        /// </summary>
        public string System1 { get; set; }
        /// <summary>
        /// 系统2
        /// </summary>
        public string System2 { get; set; }
        /// <summary>
        /// 设备及材料名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Count { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

    }
}
