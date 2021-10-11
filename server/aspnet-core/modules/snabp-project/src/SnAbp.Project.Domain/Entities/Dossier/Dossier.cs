using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;
/// <summary>
/// 卷宗
/// </summary>
namespace SnAbp.Project.Entities
{
  public class Dossier:FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 所属卷宗
        /// </summary>
        public virtual Guid ArchivesId { get; set; }

        public virtual Archives Archives { get; set; }
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
        
        public virtual FileCategory FileCategory { get; set; }
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
        public List<DossierRltFile> DossierRltFiles { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

        public Dossier()
        {

        }
        public void SetId(Guid id)
        {
            Id = id;
        }
        public Dossier(Guid id)
        {
            Id = id;
        }
    }
}
