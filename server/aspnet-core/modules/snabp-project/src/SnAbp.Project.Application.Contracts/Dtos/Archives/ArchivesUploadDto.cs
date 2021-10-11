using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Project.Dtos
{
    public class ArchivesUploadDto
    {
        public FileUploadDto File { get; set; }

        public string ImportKey { get; set; }
        public Guid ParentId { get; set; }
    }
}
