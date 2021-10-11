using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Safe.Dtos
{
   public class SafeProblemFileUploadDto
    {
        public FileUploadDto File { get; set; }

        public string ImportKey { get; set; }
    }
}