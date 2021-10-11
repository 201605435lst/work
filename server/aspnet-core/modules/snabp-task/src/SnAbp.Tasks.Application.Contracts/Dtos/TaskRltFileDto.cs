using SnAbp.File.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Tasks.Dtos
{
    public class TaskRltFileDto
    {
        public FileSimpleDto File { get; set; }
        public Guid FileId { get; set; }
    }
}
