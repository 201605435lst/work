using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Technology.Dtos
{
   public class ConstructInterfaceExportDto : FileExportDto
    {
        public ConstructInterfaceSearchDto Paramter { get; set; }
        public List<Guid> Ids { get; set; }
    }
}
