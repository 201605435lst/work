using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class StoreEquipmentSimpleSearchDto
    {
        /// <summary>
        /// 仓库 Id
        /// </summary>
        public Guid StoreHouseId { get; set; }

        /// <summary>
        /// 库存编码
        /// </summary>
        public string Code { get; set; }
    }
}
