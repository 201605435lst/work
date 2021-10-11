using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public  class MaterialOfBillRltMaterialDto : EntityDto<Guid>
    {

        public virtual MaterialOfBillDto MaterialOfBill { get; set; }
        public virtual Guid MaterialOfBillId { get; set; }

        public InventoryDto Inventory { get; set; }
        public virtual Guid? InventoryId { get; set; }

        /// <summary>
        /// 领料量
        /// </summary>
        public virtual decimal Count { get; set; }
    }
}
