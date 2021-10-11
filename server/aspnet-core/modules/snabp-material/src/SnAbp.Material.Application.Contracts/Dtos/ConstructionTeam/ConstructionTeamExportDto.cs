using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Material.Dtos
{
  public class ConstructionTeamExportDto : FileExportDto
    {
        public ConstructionTeamSearchDto Paramter { get; set; }
    }
}
