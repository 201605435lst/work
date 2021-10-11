using SnAbp.Project.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Project.Dtos
{
    public class DossierUpdateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 所属卷宗
        /// </summary>
        public virtual Guid ArchivesId { get; set; }


        /// <summary>
        /// 文件编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        public string PersonName { get; set; }
        /// <summary>
        /// 文件题名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 页号
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public virtual Guid FileCategoryId { get; set; }

        /// <summary>
        /// 文件日期
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<DossierRltFile> DossierRltFiles { get; set; } = new List<DossierRltFile>();
    }
}
