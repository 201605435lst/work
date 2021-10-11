using SnAbp.Bpm.Entities;
using SnAbp.Material.Dtos;
using System;

namespace SnAbp.Material.Dtos
{
    public class PurchasePlanRltFlowDto : SingleFlowRltEntity
    {
        public virtual Guid PurchaseId { get; set; }
        public virtual PurchasePlanDto Purchase { get; set; }
    }
}
