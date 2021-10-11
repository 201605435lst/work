using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.Identity.Dtos
{
    public class ImportData
    {
        public FileUploadDto File { get; set; }

        public string ImportKey { get; set; }
    }
}
