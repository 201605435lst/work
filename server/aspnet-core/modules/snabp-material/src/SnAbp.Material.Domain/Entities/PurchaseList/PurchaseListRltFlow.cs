using SnAbp.Bpm.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Material.Entities
{
    public class PurchaseListRltFlow : SingleFlowRltEntity
    {
        public PurchaseListRltFlow(Guid id) => Id = id;
        public virtual Guid PurchaseListId { get; set; }
        public virtual PurchaseList PurchaseList { get; set; }
    }
}
