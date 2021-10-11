using SnAbp.File.Dtos.Tag;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.File.Dtos
{
    public class FileSimpleDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Url { get; set; }

        /// <summary>
        /// 文件夹标签
        /// </summary>
        public virtual List<FileRltTagDto> Tags { get; set; }
    }
}
