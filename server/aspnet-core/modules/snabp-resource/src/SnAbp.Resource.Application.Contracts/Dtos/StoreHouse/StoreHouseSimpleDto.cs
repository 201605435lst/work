using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class StoreHouseSimpleDto:EntityDto<Guid>
    {
        public string Name { get; set; }

    }
}
