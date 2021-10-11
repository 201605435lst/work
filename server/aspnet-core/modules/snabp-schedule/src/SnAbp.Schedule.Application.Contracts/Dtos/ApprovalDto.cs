using SnAbp.Identity;
using SnAbp.Schedule.Entities;
using SnAbp.Schedule.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Schedule.Dtos
{
    public class ApprovalDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 施工单位
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// 施工日期
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 施工部位
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 专业
        /// </summary>
        public Guid ProfessionId { get; set; }
        public virtual DataDictionary Profession { get; set; }

        /// <summary>
        /// 施工计划
        /// </summary>
        public Guid ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }

        /// <summary>
        /// 计划任务名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 负责人+施工员关联表
        /// </summary>
        public List<ApprovalRltMember> ApprovalRltMembers { get; set; } = new List<ApprovalRltMember>();
        /// <summary>
        /// 负责人
        /// </summary>
        public List<ApprovalRltMember> Directors { get; set; } = new List<ApprovalRltMember>();
        /// <summary>
        /// 施工员
        /// </summary>
        public List<ApprovalRltMember> Builders { get; set; } = new List<ApprovalRltMember>();
        /// <summary>
        /// 劳务员个数
        /// </summary>
        public int MemberNum { get; set; }

        /// <summary>
        /// 关联技术资料  -- 辅助信息
        /// </summary>
        public List<ApprovalRltFile> ApprovalRltFiles { get; set; } = new List<ApprovalRltFile>();

        /// <summary>
        /// 临时设备
        /// </summary>
        public Guid TemporaryEquipmentId { get; set; }
        public virtual DataDictionary TemporaryEquipment { get; set; }

        /// <summary>
        /// 安全注意事项
        /// </summary>
        public Guid SafetyCautionId { get; set; }
        public virtual DataDictionary SafetyCaution { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 关联物资信息
        /// </summary>
        public List<ApprovalRltMaterialDto> ApprovalRltMaterials { get; set; } = new List<ApprovalRltMaterialDto>();
        /// <summary>
        /// 辅助
        /// </summary>
        public List<ApprovalRltMaterialDto> MaterialList { get; set; } = new List<ApprovalRltMaterialDto>();
        /// <summary>
        /// 器具
        /// </summary>
        public List<ApprovalRltMaterialDto> ApplianceList { get; set; } = new List<ApprovalRltMaterialDto>();
        /// <summary>
        /// 机械
        /// </summary>
        public List<ApprovalRltMaterialDto> MechanicalList { get; set; } = new List<ApprovalRltMaterialDto>();
        /// <summary>
        /// 安防信息
        /// </summary>
        public List<ApprovalRltMaterialDto> SecurityProtectionList { get; set; } = new List<ApprovalRltMaterialDto>();

        /// <summary>
        /// 状态
        /// </summary>
        public StatusType State { get; set; }


    }
}
