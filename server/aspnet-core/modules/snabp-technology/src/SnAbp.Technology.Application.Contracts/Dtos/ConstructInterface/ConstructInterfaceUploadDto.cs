using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Technology.Dtos
{
  public  class ConstructInterfaceUploadDto
    {
        public FileUploadDto File { get; set; }
        public string ImportKey { get; set; }
    }
}
