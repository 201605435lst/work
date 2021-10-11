using SnAbp.StdBasic.Dtos.Model.ModelMVD;
using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic.Dtos.Export
{  
    public class MVDData : FileExportDto
    {
        public MVDSearchDto Paramter { get; set; }
    }
}
