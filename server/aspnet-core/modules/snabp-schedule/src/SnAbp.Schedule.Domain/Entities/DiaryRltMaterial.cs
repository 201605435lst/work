using SnAbp.Schedule.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Schedule.Entities
{
    public class DiaryRltMaterial : FullAuditedEntity<Guid>
    {
        public DiaryRltMaterial(Guid id) => Id = id;
        public DiaryRltMaterial()
        {

        }
        /// <summary>
        /// 日志id
        /// </summary>
        public virtual Diary Diary { get; set; }
        public virtual Guid? DiaryId { get; set; }


        /// <summary>
        /// 材料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpecModel { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 材料类别
        /// </summary>
        public MaterialsType Type { get; set; }

    }
}
