/**********************************************************************
*******命名空间： SnAbp.Construction.Dtos.Plan.PlanContent
*******类 名 称： Analog
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 8/6/2021 2:20:06 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Construction.Dtos.Plan.PlanContent
{
    /// <summary>
    ///  进度模拟返回结果
    /// </summary>
    public class AnalogDto
    {
        // 设备分组
        public string Group { get; set; }
        // 设备命名
        public string EquipmentName { get; set; }
    }
}
