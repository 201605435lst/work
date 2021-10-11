using SnAbp.Project.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Project.Dtos
{
    public class FileClassificationDto : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 上级分类
        /// </summary>
        public virtual Guid? ParentId { get; set; }

        public List<FileClassificationDto> Children { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否文件夹
        /// </summary>
        public bool IsFile { get; set; }
        public FileClassificationDto(Guid id)
        {
            Id = id;
        }
    }
}
