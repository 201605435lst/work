using GenerateLibrary;
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Construction.Entities
{
    /// <summary>
    /// 派工单模板
    /// </summary>
    [Comment("派工单模板")]
    public class DispatchTemplate : FullAuditedEntity<Guid>
    {
        protected DispatchTemplate() { }
        public DispatchTemplate(Guid id) => Id = id;
        /// <summary>
        /// 模板名称
        /// </summary>
        [Display(DisplayType.String)]
        [Search(SearchType.BlurSearch)]
        [Create(CreateType.StringInput)]
        [Comment("模板名称")]
        public string Name { get; set; }

        /// <summary>
        /// 模板说明
        /// </summary>
        [Display(DisplayType.String)]
        [Search(SearchType.BlurSearch)]
        [Create(CreateType.StringInput)]
        [Comment("模板说明")]
        public string Description { get; set; }

        /// <summary>
        /// 是否默认
        /// </summary>
        [Display(DisplayType.Bool)]
        [Create(CreateType.BoolSwitch)]
        [Comment("是否默认")]
        public bool IsDefault { get; set; }


        /// <summary>
        /// 模板备注
        /// </summary>
        [Display(DisplayType.String)]
        [Create(CreateType.StringInput)]
        [Comment("模板备注")]
        public string Remark { get; set; }

        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
