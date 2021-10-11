using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class EquipmentGetListDto : PagedResultDto<EquipmentDto>
    {
        public int WaitingStorageCount { get; set; }


        public List<EquipmentDto> WaitingStorages { get; set; }
    }
}
