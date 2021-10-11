using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Resource.Dtos.Export
{
    public class EquipmentData : FileExportDto
    {
        public EquipmentSearchDto Paramter { get; set; }
    }
}
