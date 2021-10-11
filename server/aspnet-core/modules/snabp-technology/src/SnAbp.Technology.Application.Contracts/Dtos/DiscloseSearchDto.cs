/**********************************************************************
*******命名空间： SnAbp.Technology.Dtos
*******类 名 称： DiscloseSearchDto
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/3/30 14:07:21
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Application.Dtos;

namespace SnAbp.Technology.Dtos
{
    /// <summary>
    /// 搜索
    /// </summary>
    public class DiscloseSearchDto : PagedAndSortedResultRequestDto
    {
        public string? KeyWord { get; set; }
        public DiscloseType Type { get; set; }
    }
}
