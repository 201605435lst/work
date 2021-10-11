using System;
using System.Collections.Generic;

namespace SnAbp.Resource.Dtos
{
    public class EquipmentUpdateLimitDto
    {
        public List<Guid> Ids { get; set; }

        public int LimitUseYear { get; set; }
    }
}
