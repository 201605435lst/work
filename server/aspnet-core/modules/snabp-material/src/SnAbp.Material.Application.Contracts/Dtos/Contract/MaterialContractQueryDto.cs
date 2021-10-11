/**********************************************************************
*******命名空间： SnAbp.Material.Dtos.Contract
*******类 名 称： ContractQueryDto
*******类 说 明： 合同查询
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/3/10 9:42:13
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    /// <summary>
    /// $$
    /// </summary>
    public class ContractSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWords { get; set; }
    }
}
