/**********************************************************************
*******命名空间： SnAbp.Oa.Entities
*******类 名 称： DutySchedule
*******类 说 明： 值班表实体
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/27 16:04:43
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using SnAbp.Identity;
using Volo.Abp.Domain.Entities;
using SnAbp.MultiProject.MultiProject;

namespace SnAbp.Oa.Entities
{
    public class DutySchedule:Entity<Guid>
    {
        public DutySchedule(Guid id) => Id = id;

        /// <summary>
        /// 组织机构id
        /// </summary>
        [NotNull]public Guid OrganizationId { get; set; }
        /// <summary>
        /// 组织机构
        /// </summary>
        [NotNull] public Organization Organization { get; set; }

        /// <summary>
        /// 人员
        /// </summary>
        public List<DutyScheduleRltUser> DutyScheduleRltUsers { get; set; }

        /// <summary>
        /// 值班时间
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

    }
}
