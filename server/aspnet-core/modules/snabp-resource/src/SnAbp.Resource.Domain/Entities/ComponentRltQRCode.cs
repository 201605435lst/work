

/************************************************************************************
*命名空间：SnAbp.Resource.Entities
*文件名：ComponentRltQRCode
*创建人： liushengtao
*创建时间：2021/6/24 10:01:37
*描述：构件跟踪二维码
*
***********************************************************************/
using SnAbp.Resource.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Resource.Entities
{
  public class ComponentRltQRCode : Entity<Guid>
    {
        public ComponentRltQRCode() { }
        public ComponentRltQRCode(Guid id)
        {
            Id = id;
        }
        /// <summary>
        /// 生成设备
        /// </summary>
        public virtual Guid? GenerateEquipmentId { get; set; }
        public virtual Equipment GenerateEquipment { get; set; }
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
        /// 项目id
        /// </summary>
        public Guid? ProjectTagId { get; set; }

    }
}
