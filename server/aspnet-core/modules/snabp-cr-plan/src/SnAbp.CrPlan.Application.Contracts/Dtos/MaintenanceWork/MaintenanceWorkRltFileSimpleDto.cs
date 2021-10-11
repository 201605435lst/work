using SnAbp.File;
using SnAbp.File.Dtos;
using System.Collections.Generic;

namespace SnAbp.CrPlan.Dtos.MaintenanceWork
{
    public class MaintenanceWorkRltFileSimpleDto
    {
        public List<FileSimpleDto> ContentFiles { get; set; }

        public FileSimpleDto CoverFile { get; set; }
    }
}
