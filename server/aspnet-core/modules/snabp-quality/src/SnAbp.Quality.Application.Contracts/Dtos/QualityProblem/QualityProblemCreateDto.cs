using SnAbp.Identity;
using System;
using System.Collections.Generic;

namespace SnAbp.Quality.Dtos
{
    public class QualityProblemCreateDto
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

        /// <summary>
        /// 检查时间
        /// </summary>
        public virtual DateTime? CheckTime { get; set; }
        /// <summary>
        /// 限制整改时间
        /// </summary>
        public virtual DateTime? LimitTime { get; set; }
        /// <summary>
        /// 检查单位
        /// </summary>        
        public virtual Guid? CheckUnitId { get; set; }
        /// <summary>
        /// 检查单位名称
        /// </summary>        
        public string CheckUnitName { get; set; }
        /// <summary>
        /// 检查人
        /// </summary>
        public virtual Guid CheckerId { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        public virtual Guid? ResponsibleUserId { get; set; }
        /// <summary>
        /// 问题验证人
        /// </summary>
        public virtual Guid? VerifierId { get; set; }
        /// <summary>
        /// 责任单位
        /// </summary>
        public virtual string ResponsibleUnit { get; set; }
        /// <summary>
        /// 责任部门
        /// </summary>        
        public virtual Guid? ResponsibleOrganizationId { get; set; }

        /// <summary>
        /// 问题状态
        /// </summary>
        public virtual QualityProblemState State { get; set; }

        public virtual List<QualityProblemRltCcUserCreateDto> CcUsers { get; set; } = new List<QualityProblemRltCcUserCreateDto>();
        /// <summary>
        /// 问题文件
        /// </summary>
        public virtual List<QualityProblemRltFileCreateDto> Files { get; set; } = new List<QualityProblemRltFileCreateDto>();
        /// <summary>
        /// 关联模型
        /// </summary>
        public virtual List<QualityProblemRltEquipmentCreateDto> Equipments { get; set; } = new List<QualityProblemRltEquipmentCreateDto>();
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
