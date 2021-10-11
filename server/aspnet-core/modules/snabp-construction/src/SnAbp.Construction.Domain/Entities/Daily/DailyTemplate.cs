/**********************************************************************
*******命名空间： SnAbp.Construction.Entities.Daily
*******类 名 称： DailyTemplate
*******类 说 明： 施工日志模板
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 7/16/2021 9:32:27 AM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Construction.Entities
{
    /// <summary>
    ///  施工日志模板
    /// </summary>
    public class DailyTemplate : FullAuditedEntity<Guid>
    {
        protected DailyTemplate() { }
        public DailyTemplate(Guid id) => Id = id;
        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 模板说明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 设为默认
        /// </summary>
        public bool IsDefault { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
