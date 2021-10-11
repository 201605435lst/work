using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Material.Dtos
{
    public class MaterialOfBillRltMaterialCreateDto
    {
        public virtual Guid MaterialOfBillId { get; set; }

        public virtual Guid? InventoryId { get; set; }

        /// <summary>
        /// 领料量
        /// </summary>
        public virtual decimal Count { get; set; }
    }
}
