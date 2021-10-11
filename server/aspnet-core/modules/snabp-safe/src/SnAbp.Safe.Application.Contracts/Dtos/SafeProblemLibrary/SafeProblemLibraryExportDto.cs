using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Safe.Dtos
{
   public class SafeProblemLibraryExportDto : FileExportDto
    {
        public SafeProblemLibrarySearchDto Paramter { get; set; }
    }
}
