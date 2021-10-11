using SnAbp.Bpm.Entities;
using SnAbp.Material.Dtos;
using System;

namespace SnAbp.Material.Dtos
{
    public class PurchaseListRltFlowCreateDto : SingleFlowRltEntity
    {
        public virtual Guid PurchaseListId { get; set; }
    }
}
