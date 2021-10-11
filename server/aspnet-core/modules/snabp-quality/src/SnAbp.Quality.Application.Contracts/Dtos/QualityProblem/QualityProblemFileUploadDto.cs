using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Quality.Dtos
{
   public class QualityProblemFileUploadDto
    {
        public FileUploadDto File { get; set; }

        public string ImportKey { get; set; }
    }
}