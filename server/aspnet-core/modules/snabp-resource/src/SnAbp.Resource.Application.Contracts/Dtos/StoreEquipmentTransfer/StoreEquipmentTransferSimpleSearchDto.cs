using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
  public class StoreEquipmentTransferSimpleSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 仓库 Id
        /// </summary>
        public Guid StoreHouseId { get; set; }
        /// <summary>
        /// 人员Id
        /// </summary>
        public Guid UserId { get; set; }
        public IdentityUser User { get; set; }

        /// <summary>
        /// 人员姓名
        /// </summary>
        //public string UserName { get; set; }
        /// <summary>
        /// 库存编码
        /// </summary>
        public string Code { get; set; }
    }
}
