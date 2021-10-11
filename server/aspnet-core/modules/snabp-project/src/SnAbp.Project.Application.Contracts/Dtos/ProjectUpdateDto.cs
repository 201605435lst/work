using SnAbp.Identity;
using SnAbp.Project.enums;
using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.Project.Entities;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Project.Dtos
{
    public class ProjectUpdateDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 组织机构
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 项目经理，负责人
        /// </summary>
        public Guid ManagerId { get; set; }

        /// <summary>
        /// 项目描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 计划开工日期
        /// </summary>
        public DateTime PlannedStartTime { get; set; }

        /// <summary>
        /// 计划结束日期
        /// </summary>

        public DateTime PlannedEndTime { get; set; }

        /// <summary>
        /// 所属区域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 项目地址
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailAddress { get; set; }

        /// <summary>
        /// 工程工期
        /// </summary>
        public decimal ConstructionPeriod { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public Guid TypeId { get; set; }

        /// <summary>
        /// 质量等级
        /// </summary>
        public int QualityLevel { get; set; }

        /// <summary>
        /// 项目状态，中标
        /// </summary>
        public ProjectState State { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 项目进度
        /// </summary>
        public decimal Progress { get; set; }

        /// <summary>
        /// 建设规模
        /// </summary>
        public string Scale { get; set; }

        /// <summary>
        /// 项目造价
        /// </summary>
        public string Cost { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
     
        public string Remark { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public string Lat { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public string Lng { get; set; }

        /// <summary>
        /// 关联建设单位
        /// </summary>
        public List<UnitDto> Units { get; set; }

        /// <summary>
        /// 关联项目成员
        /// </summary>
        public List<Guid> ProjectRltMemberIds { get; set; } = new List<Guid>();

        /// <summary>
        /// 关联合同
        /// </summary>
        public List<Guid> ProjectRltContractIds { get; set; }

        public List<Guid> ProjectRltFileIds { get; set; }
    }
}
