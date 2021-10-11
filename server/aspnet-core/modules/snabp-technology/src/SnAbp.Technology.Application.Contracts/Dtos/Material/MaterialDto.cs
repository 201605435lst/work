/**********************************************************************
*******命名空间： SnAbp.Technology.Dtos.ConstructInterfaceInfo
*******类 名 称： Class1
*******类 说 明： 材料 dto
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/21/2021 1:50:55 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Identity;

using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Application.Dtos;

namespace SnAbp.Technology.Dtos
{
    /// <summary>
    ///  材料 dto
    /// </summary>
    public class MaterialDto:EntityDto<Guid>
    {
        public string Name { get; set; }
        public Guid TypeId { get; set; }

        // 材料类型
        public DataDictionaryDto Type { get; set; }
        public string Spec { get; set; }
        public string Model { get; set; }
        public string Unit { get; set; }

        public decimal Price { get; set; }

        public string Remark { get; set; }
    }
}
