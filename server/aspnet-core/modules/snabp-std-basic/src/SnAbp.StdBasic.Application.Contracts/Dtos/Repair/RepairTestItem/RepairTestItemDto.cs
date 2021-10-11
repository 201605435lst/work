using SnAbp.File.Dtos;
using SnAbp.StdBasic.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{

    /// <summary>
    /// 测试项表
    /// </summary>
    public class RepairTestItemDto : EntityDto<Guid>
    {
        /// <summary>
        /// 维修项id
        /// </summary>
        public Guid RepairItemId { get; set; }

        /// <summary>
        /// 测试项名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 测试项类型
        /// </summary>
        public RepairTestType Type { get; set; }

        /// <summary>
        /// 测试单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 预设值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 阈值最大值
        /// </summary>
        public float? MaxRated { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 阈值最小值
        /// </summary>
        public float? MinRated { get; set; }


        /// <summary>
        /// 文件
        /// </summary>
        public virtual Guid? FileId { get; set; }
        public virtual FileSimpleDto File { get; set; }
    }
}
