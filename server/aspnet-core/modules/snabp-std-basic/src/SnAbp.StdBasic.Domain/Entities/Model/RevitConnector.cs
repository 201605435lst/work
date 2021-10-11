using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 连接件表
    /// </summary>
    public class RevitConnector : Entity<Guid>
    {
        /// <summary>
        /// 模型文件Id
        /// </summary>
        public Guid? ModelFileId { get; set; }
        public virtual ModelFile ModelFile { get; set; }

        [MaxLength(50)]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string Position { get; set; }

        protected RevitConnector() { }
        public RevitConnector(Guid id)
        {
            Id = id;
        }
    }
}