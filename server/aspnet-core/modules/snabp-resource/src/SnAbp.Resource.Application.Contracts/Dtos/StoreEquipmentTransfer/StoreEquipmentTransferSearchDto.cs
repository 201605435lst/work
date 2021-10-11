
using SnAbp.Resource.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{ 
   public class StoreEquipmentTransferSearchDto:PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 组织机构
        /// </summary>
        public Guid OrganizationId { get; set; }
        /// <summary>
        /// 仓库 Id
        /// </summary>
        public Guid StoreHouseId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public StoreEquipmentTransferType Type { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
    }
}
