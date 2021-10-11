using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.Utils.DataImport;

namespace SnAbp.StdBasic.Dtos.Export
{
   public class QuotaCategoryData : FileExportDto
    {
        public QuotaCategoryGetListByIdsDto Paramter { get; set; }
    }
}
