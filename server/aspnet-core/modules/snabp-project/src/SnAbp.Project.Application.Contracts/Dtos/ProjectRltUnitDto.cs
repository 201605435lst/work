using System;

namespace SnAbp.Project.Dtos
{
    public class ProjectRltUnitDto
    {
        public UnitDto Unit { get; set; }
        public virtual Guid? UnitId { get; set; }

    }
}
