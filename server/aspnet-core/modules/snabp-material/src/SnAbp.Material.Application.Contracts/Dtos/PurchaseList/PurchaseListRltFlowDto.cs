using SnAbp.Bpm.Entities;
using SnAbp.Material.Dtos;
using System;

namespace SnAbp.Material.Dtos
{
    public class PurchaseListRltFlowDto : SingleFlowRltEntity
    {
        public virtual Guid PurchaseListId { get; set; }
        public virtual PurchaseListDto PurchaseList { get; set; }
    }
}
