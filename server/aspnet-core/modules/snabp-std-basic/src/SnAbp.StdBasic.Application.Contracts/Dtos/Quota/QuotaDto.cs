using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Dtos
{
    public class QuotaDto : Entity<Guid>
    {
        /// <summary>
        /// 定额分类Id
        /// </summary>
        public Guid QuotaCategoryId { get; set; }

        /// <summary>
        /// 定额分类名称
        /// </summary>
        public string QuotaCategoryName { get; set; }

        /// <summary>
        /// 定额名称
        /// </summary>
        [MaxLength(100)]
        [Description("名称")]
        public string Name { get; set; }
        /// <summary>
        /// 定额编码
        /// </summary>
        [MaxLength(50)]
        [Description("编号")]
        public string Code { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        [MaxLength(50)]
        [Description("单位")]
        public string Unit { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public float Weight { get; set; }
        /// <summary>
        /// 人工费
        /// </summary>
        public float LaborCost { get; set; }
        /// <summary>
        /// 材料费
        /// </summary>
        public float MaterialCost { get; set; }
        /// <summary>
        /// 机械使用费
        /// </summary>
        public float MachineCost { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 行政区域
        /// </summary>
        public int AreaId { get; set; }
        /// <summary>
        /// 行政区划名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// Specialty 来源于数据字典，止于专业节点（即每个专业的定额分类顶节点）
        /// </summary>
        public Guid SpecialtyId { get; set; }

        public string SpecialtyName { get; set; }

        /// <summary>
        ///标准编号 Code 来源于数据字典
        /// </summary>

        public Guid StandardCodeId { get; set; }

        /// <summary>
        ///标准编号 Code 来源于数据字典
        /// </summary>

        public string StandardCodeName { get; set; }

        /// <summary>
        /// 清单列表
        /// </summary>
        public List<QuotaItemDto> QuotaItems { get; set; }
    }
}
