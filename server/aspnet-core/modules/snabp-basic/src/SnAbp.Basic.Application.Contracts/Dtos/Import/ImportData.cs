using ICSharpCode.SharpZipLib.Zip;
using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Basic.Dtos.Import
{
    public class ImportData
    {
        public FileUploadDto File { get; set; }

        public string ImportKey { get; set; }

        public string BelongOrgCode { get; set; }
    }
}
