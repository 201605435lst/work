/**********************************************************************
*******命名空间： SnAbp.Quality.Entities
*******类 名 称： QualityProblemRltEquipment
*******类 说 明： 问题关联模型
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/4/29 19:03:13
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities;

namespace SnAbp.Quality.Entities
{
    /// <summary>
    /// 问题模型关联表
    /// </summary>
    public class QualityProblemRltEquipment : Entity<Guid>
    {
        public Guid QualityProblemId { get; set; }
        public QualityProblem QualityProblem { get; set; }
        public Guid EquipmentId { get; set; }
        public virtual Resource.Entities.Equipment Equipment { get; set; }
        public QualityProblemRltEquipment(Guid id)
        {
            Id = id;
        }
    }
}
