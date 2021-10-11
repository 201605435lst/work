using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Project.Dtos
{
    public class FileClassificationSearchDto
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
    }
}
