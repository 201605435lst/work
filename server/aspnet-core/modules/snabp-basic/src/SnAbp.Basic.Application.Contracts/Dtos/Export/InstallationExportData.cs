using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Basic.Dtos
{
    public class InstallationExportData : FileExportDto
    {
        public InstallationSiteSearchDto paramter { get; set; }
    }
}
