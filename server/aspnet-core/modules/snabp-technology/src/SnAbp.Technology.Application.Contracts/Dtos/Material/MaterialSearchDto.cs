/**********************************************************************
*******命名空间： SnAbp.Technology.Dtos.Material
*******类 名 称： MaterialSearchDto
*******类 说 明： 材料信息查询dto
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/21/2021 1:52:29 PM
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
    /// 材料信息查询 
    /// </summary>
    public class MaterialSearchDto : PagedAndSortedResultRequestDto
    {
        public string KeyWords { get; set; }
        /// <summary>
        /// 材料类型id（数据字典）
        /// </summary>
        public Guid? TypeId { get; set; }
    }
}
