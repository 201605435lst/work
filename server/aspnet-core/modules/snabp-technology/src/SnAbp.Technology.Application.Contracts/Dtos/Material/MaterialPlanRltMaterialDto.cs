/**********************************************************************
*******命名空间： SnAbp.Technology.Dtos.Material
*******类 名 称： MaterialPlanRltMaterialDto
*******类 说 明： 用料计划关联材料信息
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/21/2021 2:59:58 PM
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
    /// 用料计划关联材料信息
    /// </summary>
    public class MaterialPlanRltMaterialDto:EntityDto<Guid>
    {
        public MaterialDto Material { get; set; }
        public decimal Count { get; set; }
    }

}
