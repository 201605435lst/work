using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Basic.Dtos.Export
{
    public class RailwayExportData : FileExportDto
    {
        public RailwaySearchDto paramter { get; set; }
    }
}
