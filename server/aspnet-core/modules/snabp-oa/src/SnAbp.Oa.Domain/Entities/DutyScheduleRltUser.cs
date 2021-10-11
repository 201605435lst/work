/**********************************************************************
*******命名空间： SnAbp.Oa.Entities
*******类 名 称： DutyScheduleRltUser
*******类 说 明： 值班人员关联表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/27 16:08:41
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Identity;
using System;
using Volo.Abp.Domain.Entities;
using SnAbp.MultiProject.MultiProject;

namespace SnAbp.Oa.Entities
{
    public class DutyScheduleRltUser:Entity<Guid>
    {
        public virtual Guid UserId { get; set; }
        public virtual IdentityUser User { get; set; }
        public virtual Guid DutyScheduleId { get; set; }
        public virtual DutySchedule DutySchedule { get; set; }
        public DutyScheduleRltUser(Guid id) => Id = id;
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

    }
}
