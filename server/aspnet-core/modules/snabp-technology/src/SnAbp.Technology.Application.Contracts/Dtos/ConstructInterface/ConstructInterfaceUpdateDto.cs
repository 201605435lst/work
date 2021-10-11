using SnAbp.Technology.enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Technology.Dtos
{
    public class ConstructInterfaceUpdateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 接口名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 接口编号
        /// </summary>
        public virtual string Code { get; set; }

        //// 施工地点
        //public virtual Guid? ConstructionSectionId { get; set; }
        /// <summary>
        /// 专业 DataDictionaryId
        /// </summary>
        public virtual Guid? ProfessionId { get; set; }
        /// <summary>
        /// 接口位置
        /// </summary>
        public virtual string Position { get; set; }
        /// <summary>
        /// 接口数据
        /// </summary>
        public virtual string GisData { get; set; }
        /// <summary>
        /// 材料规格
        /// </summary>
        public string MaterialSpec { get; set; }
        /// <summary>
        /// 材料名称
        /// </summary>
        public string MarerialName { get; set; }

        /// <summary>
        /// 材料数量
        /// </summary>
        public string MarerialCount { get; set; }
        /// <summary>
        /// 土建单位
        /// </summary>
        public virtual Guid? BuilderId { get; set; }
        /// <summary>
        /// 接口检查情况
        /// </summary>
        public virtual MarkType MarkType { get; set; }
        /// <summary>
        /// 接口类型
        /// </summary>
        public virtual Guid? InterfaceManagementTypeId { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public virtual Guid? EquipmentId { get; set; }
        /// <summary>
        /// 情况标记信息表
        /// </summary>
        public virtual List<ConstructInterfaceInfoDto> ConstructInterfaceInfos { get; set; } = new List<ConstructInterfaceInfoDto>();
    }
}
