/**********************************************************************
*******命名空间： SnAbp.Technology.Dtos
*******类 名 称： QuantitiesSearchDto
*******类 说 明： 工程量查询条件
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/23/2021 3:23:38 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Application.Dtos;

namespace SnAbp.Technology.Dtos
{
    /// <summary>
    /// 工程量查询条件 
    /// </summary>
    public class QuantitiesSearchDto: PagedAndSortedResultRequestDto
    {
        public bool Statistic { get; set; }//  是否分析
        public string KeyWords { get; set; }
        public Guid SpecId { get; set; }
    }
}
