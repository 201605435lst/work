using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Alarm.Dtos
{
    public class ImportData
    {
        public FileUploadDto File { get; set; }

        public string ImportKey { get; set; }
    }
}
