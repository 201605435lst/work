using SnAbp.StdBasic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 模型文件表
    /// </summary>
    public class ModelFile : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 模型Id
        /// </summary>
        public Guid? ModelId { get; set; }
        public virtual Model Model { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        //public string Thumb { get; set; }
       
        public virtual File.Entities.File Thumb { get; set; }
        public Guid? ThumbId { get; set; }

        ///// <summary>
        ///// 路径
        ///// </summary>
        //public string Url { get; set; }

        /// <summary>
        /// 族文件Id
        /// </summary>
        public virtual File.Entities.File FamilyFile { get; set; }
        public Guid? FamilyFileId { get; set; }

        /// <summary>
        /// 模型精细等级
        /// </summary>
        public ModelDetailLevel DetailLevel { get; set; }

        /// <summary>
        /// Revit 连接件
        /// </summary>
        public virtual List<RevitConnector> RevitConnectors { get; set; }

        protected ModelFile() { }
        public ModelFile(Guid id)
        {
            Id = id;
        }
    }
}