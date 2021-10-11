using SnAbp.Project.enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Project.Dtos
{
    public  class ArchivesUpdateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 所属目录
        /// </summary>
        public virtual Guid ArchivesCategoryId { get; set; }

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
    }
}
