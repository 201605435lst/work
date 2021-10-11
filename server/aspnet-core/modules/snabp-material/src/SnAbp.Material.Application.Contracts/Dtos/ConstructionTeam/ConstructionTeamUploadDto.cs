using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Material.Dtos
{
   public class ConstructionTeamUploadDto
    {
        public FileUploadDto File { get; set; }
        public string ImportKey { get; set; }
    }
}
