using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Resource.Dtos.Export
{
    public class EquipmentGroupData : FileExportDto
    {
        public EquipmentGroupGetListDto paramter { get; set; }
    }
}
