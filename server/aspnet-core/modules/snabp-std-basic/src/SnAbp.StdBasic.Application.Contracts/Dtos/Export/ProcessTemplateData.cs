using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic.Dtos.Export
{
  public  class ProcessTemplateData : FileExportDto
    {
        public ProcessTemplateGetListByIdsDto Paramter { get; set; }
    }
}
