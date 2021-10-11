using SnAbp.Bpm.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Material.Entities
{
    public class PurchasePlanRltFlow : SingleFlowRltEntity
    {
        public PurchasePlanRltFlow(Guid id) => Id = id;
        public virtual Guid PurchasePlanId { get; set; }
        public virtual PurchasePlan PurchasePlan { get; set; }
    }
}
