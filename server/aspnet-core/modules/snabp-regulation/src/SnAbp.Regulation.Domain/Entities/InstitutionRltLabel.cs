/**********************************************************************
*******命名空间： SnAbp.Regulation.Entities
*******类 名 称： InstitutionRltLabel
*******类 说 明： 制度标签关联表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/17 16:14:50
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Regulation.Entities
{
    public class InstitutionRltLabel : Entity<Guid>
    {
        public InstitutionRltLabel(Guid id) { Id = id; }

        public virtual Guid InstitutionId { get; set; }
        public virtual Institution Institution { get; set; }

        public virtual Guid LabelId { get; set; }
        public virtual Label Label { get; set; }

        public override object[] GetKeys() => new object[] { this.InstitutionId, this.LabelId };
    }
}
