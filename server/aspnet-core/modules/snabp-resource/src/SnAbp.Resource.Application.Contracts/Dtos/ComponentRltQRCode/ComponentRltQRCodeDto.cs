
using SnAbp.Resource.Entities;
using SnAbp.Resource.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
  public class ComponentRltQRCodeDto : EntityDto<Guid>
    {
        /// <summary>
        /// 生成设备
        /// </summary>
        public virtual Guid? GeneratingEquipmentId { get; set; }
        public virtual Equipment GeneratingEquipment { get; set; }
        /// <summary>
        /// 安装设备
        /// </summary>
        public virtual Guid? InstallationEquipmentId { get; set; }
        public virtual Equipment InstallationEquipment { get; set; }
        /// <summary>
        /// 激活状态
        /// </summary>
        public ActivatedState State { get; set; }
        /// <summary>
        /// 跟踪构件二维码 QRCode（构件分类code@新id）
        /// </summary>
        public string QRCode { get; set; }
        /// <summary>
        /// 跟踪记录信息
        /// </summary>
        public List<ComponentTrackRecord> ComponentTrackRecord { get; set; } = new List<ComponentTrackRecord>();

    }
}
