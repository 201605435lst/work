using SnAbp.Resource.Dtos;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Quality.Dtos
{
    public class QualityProblemRltEquipmentDto : EntityDto<Guid>
    {
        public Guid QualityProblemId { get; set; }
        public QualityProblemDto QualityProblem { get; set; }
        public virtual Guid EquipmentId { get; set; }
        public virtual EquipmentDto Equipment { get; set; }
    }
}
