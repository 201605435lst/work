using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using SnAbp.Project.enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;
/// <summary>
/// 档案
/// </summary>
namespace SnAbp.Project.Entities
{
    public class Archives : FullAuditedEntity<Guid>
    {
     
        /// <summary>
        /// 所属档案
        /// </summary>
        public virtual Guid ArchivesCategoryId { get; set; }
        public virtual  ArchivesCategory ArchivesCategory { get; set; }

        /// <summary>
        /// 宗号
        /// </summary>
        public string FileCode { get; set; }
        /// <summary>
        /// 档号
        /// </summary>
        public string ProjectCode { get; set; }
        /// <summary>
        /// 案卷号
        /// </summary>
        public string ArchivesFilesCode { get; set; }
        /// <summary>
        /// 案卷分类
        /// </summary>
        public virtual Guid BooksClassificationId { get; set; }
        public virtual BooksClassification BooksClassification { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public string Year { get; set; }
        /// <summary>
        /// 案卷题名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 密级
        /// </summary>
        public Security Security { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 份数
        /// </summary>
        public int Copies { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 编制日期
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 编制单位
        /// </summary>
        public string Unit { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

        ///// <summary>
        ///// 项目
        ///// </summary>
        //public virtual Guid ProjectId { get; set; }
        //public virtual Project Project { get; set; }


        public Archives()
        {

        }
        public void SetId(Guid id)
        {
            Id = id;
        }
        public Archives(Guid id)
        {
            Id = id;
        }
    }
}
