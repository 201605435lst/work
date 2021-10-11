using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Project.Dtos
{
   public class ArchivesCategoryLockDto : EntityDto<Guid>
    {
        /// <summary>
        /// 是否加密
        /// </summary>
        public bool IsEncrypt { get; set; }
    }
}
