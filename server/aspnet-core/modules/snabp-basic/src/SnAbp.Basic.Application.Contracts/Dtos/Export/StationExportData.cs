using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Basic.Dtos.Export
{
    public class StationExportData : FileExportDto
    {
        public StationSearchDto paramter { get; set; }
    }
}
