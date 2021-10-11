/**********************************************************************
*******命名空间： SnAbp.Technology.Dtos.Material
*******类 名 称： MaterialCreateDto
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/21/2021 1:53:56 PM
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
    ///  材料信息创建dto
    /// </summary>
    public class MaterialPlanCreateDto : EntityDto<Guid>
    {
        public string PlanName { get; set; }
        public DateTime PlanDate { get; set; }
        public List<MaterialRltDto> Materials { get; set; }
    }
    public class MaterialRltDto
    {
        public Guid Id { get; set; }
        public decimal Count { get; set; }
    }
}
