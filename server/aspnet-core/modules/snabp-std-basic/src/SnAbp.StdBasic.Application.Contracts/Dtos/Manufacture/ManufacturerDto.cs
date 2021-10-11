using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class ManufacturerDto : EntityDto<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 铁路编码
        /// </summary>
        public string CSRGCode { get; set; }

        /// <summary>
        /// 上级厂家
        /// </summary>
        public Guid? ParentId { get; set; }
        public List<ManufacturerDto> Children { get; set; } = new List<ManufacturerDto>();

        /// <summary>
        /// 负责人
        /// </summary>
        public string Principal { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 厂家地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        public List<EquipmentControlTypeSimpleDto> EquipmentTypes { get; set; } = new List<EquipmentControlTypeSimpleDto>();
    }
}
