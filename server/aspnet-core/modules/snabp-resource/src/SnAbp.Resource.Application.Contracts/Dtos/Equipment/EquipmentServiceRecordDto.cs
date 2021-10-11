using SnAbp.Identity;
using SnAbp.Resource.Enums;
using SnAbp.StdBasic.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SnAbp.Basic.Dtos;
using Volo.Abp.Application.Dtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnAbp.Resource.Dtos
{
    public class EquipmentServiceRecordDto : EntityDto<Guid>
    {
        /// <summary>
        /// 设备 Id
        /// </summary>
        public Guid EquipmentId { get; set; }
        public EquipmentDto Equipment { get; set; }

        /// <summary>
        /// 库存设备 Id
        /// </summary>
        public Guid StoreEquipmentId { get; set; }
        public StoreEquipmentDto StoreEquipment { get; set; }

        /// <summary>
        /// 记录类型
        /// </summary>
        public EquipmentServiceRecordType Type { get; set; }

        /// <summary>
        /// 关联人员Id
        /// </summary>
        public Guid? UserId { get; set; }
        public IdentityUser User { get; set; }

        /// <summary>
        /// 人员姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }
    }
}
