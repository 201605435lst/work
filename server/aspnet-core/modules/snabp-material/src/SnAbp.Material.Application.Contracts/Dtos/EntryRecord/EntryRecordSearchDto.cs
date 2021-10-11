/**********************************************************************
*******命名空间： SnAbp.Material.Dtos.Inventory
*******类 名 称： InventorySearchDto
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/2/4 10:23:19
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    /// <summary>
    /// $$
    /// </summary>
    public class EntryRecordSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string Keyword { get; set; }
        public DateTime? STime { get; set; }
        public DateTime? ETime { get; set; }
        /// <summary>
        /// 库存位置
        /// </summary>
        public Guid? PartitionId { get; set; }
    }
}
