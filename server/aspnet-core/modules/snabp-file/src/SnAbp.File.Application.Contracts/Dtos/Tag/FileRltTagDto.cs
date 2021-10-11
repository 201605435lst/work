using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.File.Dtos.Tag
{
    public class FileRltTagDto : EntityDto<Guid>
    {
        public virtual Guid TagId { get; set; }

        public virtual Guid FileId { get; set; }
        /// <summary>
        ///     标签类
        /// </summary>
        public virtual FileTagDto Tag { get; set; }

        /// <summary>
        ///     文件类
        /// </summary>
        public virtual FileSimpleDto File { get; set; }
    }
}
