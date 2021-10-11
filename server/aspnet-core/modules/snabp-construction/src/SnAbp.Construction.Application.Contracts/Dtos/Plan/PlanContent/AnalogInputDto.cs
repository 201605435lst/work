/**********************************************************************
*******命名空间： SnAbp.Construction.Dtos.Plan.PlanContent
*******类 名 称： AnalogInputDto
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 8/6/2021 2:11:55 PM
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
    /// 模拟数据查询条件  
    /// </summary>
    public class AnalogInputDto
    {
        public Guid PlanId { get; set; }
        public AnalogType Type { get; set; }
        public string Date { get; set; }
        public int Speed { get; set; }
    }


    // 数据模式方式
    public enum AnalogType
    {
        Day,
        Week,
        Month,
        Quarter,
        HalfYear
    }
}
