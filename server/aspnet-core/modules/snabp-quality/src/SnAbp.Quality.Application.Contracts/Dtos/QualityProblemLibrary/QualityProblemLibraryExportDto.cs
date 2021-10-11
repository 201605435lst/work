using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Quality.Dtos
{
  public  class QualityProblemLibraryExportDto : FileExportDto
    {
        public QualityProblemLibrarySearchDto Paramter { get; set; }
    }
}
