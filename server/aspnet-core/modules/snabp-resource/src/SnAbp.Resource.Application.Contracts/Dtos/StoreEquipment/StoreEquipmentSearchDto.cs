using SnAbp.Resource.Enums;
using SnAbp.StdBasic.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class StoreEquipmentSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 库存编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 产品分类
        /// </summary>
        public Guid ProductCategoryId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public StoreEquipmentState State { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public Guid? StoreHouseId { get; set; }
        /// <summary>
        /// 组织机构
        /// </summary>
        public Guid? OrganizationId { get; set; }
    }
}
