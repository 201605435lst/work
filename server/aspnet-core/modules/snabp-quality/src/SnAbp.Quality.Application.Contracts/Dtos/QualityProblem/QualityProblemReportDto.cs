using SnAbp.Quality.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Quality.Dtos
{
    public class QualityProblemReportDto : EntityDto<Guid>
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
        public virtual QualityProblemType Type { get; set; }

        /// <summary>
        /// 检查时间
        /// </summary>
        public virtual DateTime? CheckTime { get; set; }
        /// <summary>
        /// 限制整改时间
        /// </summary>
        public virtual DateTime? LimitTime { get; set; }
        /// <summary>
        /// 检查人
        /// </summary>
        public virtual Identity.IdentityUser Checker { get; set; }
        public virtual Guid CheckerId { get; set; }



        /// <summary>
        /// 责任人
        /// </summary>
        public virtual Guid? ResponsibleUserId { get; set; }
        public virtual Identity.IdentityUser ResponsibleUser { get; set; }
        /// <summary>
        /// 问题验证人
        /// </summary>
        public virtual Identity.IdentityUser Verifier { get; set; }
        public virtual Guid? VerifierId { get; set; }

        /// <summary>
        /// 责任单位
        /// </summary>
        public virtual string ResponsibleUnit { get; set; }
        /// <summary>
        /// 责任部门
        /// </summary>        
        public virtual Identity.Organization ResponsibleOrganization { get; set; }
        public virtual Guid? ResponsibleOrganizationId { get; set; }

        /// <summary>
        /// 问题状态
        /// </summary>
        public virtual QualityProblemState State { get; set; }

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
        /// <summary>
        /// 整改、验证记录
        /// </summary>
        public virtual List<QualityProblemRecord> QualityProblemRecord { get; set; } = new List<QualityProblemRecord>();
        /// <summary>
        /// 整改记录
        /// </summary>
        public virtual List<QualityProblemRecord> ImproveRecord { get; set; } = new List<QualityProblemRecord>();
        /// <summary>
        /// 验证记录
        /// </summary>
        public virtual List<QualityProblemRecord> VerifyRecord { get; set; } = new List<QualityProblemRecord>();

    }
}
