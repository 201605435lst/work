using SnAbp.CrPlan.Dto;
using SnAbp.File;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dtos
{
    public class MaintenanceWorkRltFileDto : EntityDto<Guid>
    {
        /// <summary>
        /// 维修作业
        /// </summary>
        public Guid MaintenanceWorkId { get; set; }
        public virtual MaintenanceWorkDto MaintenanceWork { get; set; }

        /// <summary>
        /// 文件(方案)
        /// </summary>
        public Guid FileId { get; set; }
        public virtual File.Dtos.FileDownloadDto File { get; set; }

        /// <summary>
        /// 文件关联的文件(方案封面)
        /// </summary>
        public Guid? RelateFileId { get; set; }
        public virtual File.Dtos.FileDownloadDto RelateFile { get; set; }
    }

    /// <summary>
    /// 维修作业关联方案dto
    /// </summary>
    public class MaintenanceWorkRltFileCreateDto
    {
        /// <summary>
        /// 文件(方案)
        /// </summary>
        public List<FileDomainDto> ContentFiles { get; set; }

        /// <summary>
        /// 文件关联的文件(方案封面)
        /// </summary>
        public FileDomainDto CoverFile { get; set; }
    }
}
