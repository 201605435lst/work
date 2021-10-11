using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
   public class StoreHouseUpdateEnableDto :EntityDto<Guid>
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; } = true;
    }
}
