using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Emerg.Entities
{
    /// <summary>
    /// 应急预案记录表
    /// </summary>
    public class EmergPlanRecord : FullAuditedEntity<Guid>
    {
        protected EmergPlanRecord() { }
        public EmergPlanRecord(Guid id) { Id = id; }

        /// <summary>
        /// 预案名称
        /// </summary>
        [MaxLength(120)]
        public string Name { get; set; }

        /// <summary>
        /// 预案等级
        /// </summary>
        public Guid LevelId { get; set; }
        public virtual DataDictionary Level { get; set; }


        [InverseProperty("EmergPlanRecord")]
        /// <summary>
        /// 构件分类
        /// </summary>
        public List<EmergPlanRecordRltComponentCategory> EmergPlanRecordRltComponentCategories { get; set; }

        /// <summary>
        /// 预案摘要
        /// </summary>
        [MaxLength(1000)]
        public string Summary { get; set; }

       
        /// <summary>
        /// 预案流程
        /// </summary>
        public string Flow { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(120)]
        public string Remark { get; set; }

        [InverseProperty("EmergPlanRecord")]
        /// <summary>
        /// 主要附件
        /// </summary>
        public List<EmergPlanRecordRltFile> EmergPlanRecordRltFiles { get; set; }

        /// <summary>
        /// 图文资料
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 成员记录表
        /// </summary>
        public List<EmergPlanRecordRltMember> EmergPlanRecordRltMembers { get; set; }

        /// <summary>
        /// 预案处理记录表
        /// </summary>
        public List<EmergPlanProcessRecord> ProcessRecords { get; set; }
        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
