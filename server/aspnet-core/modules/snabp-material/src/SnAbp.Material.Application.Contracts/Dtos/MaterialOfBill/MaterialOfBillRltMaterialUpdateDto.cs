using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class MaterialOfBillRltMaterialUpdateDto 
    {

        public virtual Guid? InventoryId { get; set; }

        /// <summary>
        /// 领料量
        /// </summary>
        public virtual decimal count { get; set; }
    }
}
