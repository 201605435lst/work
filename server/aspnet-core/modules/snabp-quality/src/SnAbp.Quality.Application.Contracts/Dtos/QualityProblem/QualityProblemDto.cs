using SnAbp.Identity;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Quality.Dtos
{
    public class QualityProblemDto : EntityDto<Guid>
    {
        /// <summary>
        /// 问题编码-自动生成：事件类型首字母+时间 （待定）
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        /// 问题标题
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 问题类型
        /// </summary>
        public QualityProblemType Type { get; set; }
        /// <summary>
        /// 问题等级
        /// </summary>
        public QualityProblemLevel Level { get; set; }
        /// <summary>
        /// 所属专业，数字字典
        /// </summary>
        public virtual Guid ProfessionId { get; set; }
        public virtual DataDictionaryDto Profession { get; set; }

        /// <summary>
        /// 检查时间
        /// </summary>
        public virtual DateTime? CheckTime { get; set; }
        /// <summary>
        /// 限制整改时间
        /// </summary>
        public virtual DateTime? LimitTime { get; set; }
        /// <summary>
        /// 最近整改时间
        /// </summary>
        public virtual DateTime? ChangeTime { get; set; }
        /// <summary>
        /// 检查单位
        /// </summary>        
        public virtual Guid? CheckUnitId { get; set; }
        public virtual OrganizationDto CheckUnit { get; set; }
        /// <summary>
        /// 检查单位名称
        /// </summary>        
        public string CheckUnitName { get; set; }
        /// <summary>
        /// 检查人
        /// </summary>
        public virtual Guid CheckerId { get; set; }
        public virtual IdentityUserDto Checker { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        public virtual Guid? ResponsibleUserId { get; set; }
        public virtual IdentityUserDto ResponsibleUser { get; set; }
        /// <summary>
        /// 问题验证人
        /// </summary>
        public virtual Guid? VerifierId { get; set; }
        public virtual IdentityUserDto Verifier { get; set; }
        /// <summary>
        /// 责任单位
        /// </summary>
        public virtual string ResponsibleUnit { get; set; }
        /// <summary>
        /// 责任部门
        /// </summary>        
        public virtual Guid? ResponsibleOrganizationId { get; set; }
        public virtual OrganizationDto ResponsibleOrganization { get; set; }

        /// <summary>
        /// 问题状态
        /// </summary>
        public virtual QualityProblemState State { get; set; }

        /// <summary>
        /// 抄送我的人员列表
        /// </summary>
        public virtual List<QualityProblemRltCcUserDto> CcUsers { get; set; } = new List<QualityProblemRltCcUserDto>();
        /// <summary>
        /// 问题文件
        /// </summary>
        public virtual List<QualityProblemRltFileDto> Files { get; set; } = new List<QualityProblemRltFileDto>();
        /// <summary>
        /// 关联模型
        /// </summary>
        public virtual List<QualityProblemRltEquipmentDto> Equipments { get; set; } = new List<QualityProblemRltEquipmentDto>();
        /// <summary>
        /// 问题内容
        /// </summary>
        public virtual string Content { get; set; }
        /// <summary>
        /// 整改措施，整改意见
        /// </summary>
        public virtual string Suggestion { get; set; }
    }
}
