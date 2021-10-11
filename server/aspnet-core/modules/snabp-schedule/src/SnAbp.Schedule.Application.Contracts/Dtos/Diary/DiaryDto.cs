using SnAbp.Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Schedule.Dtos
{
   public class DiaryDto : EntityDto<Guid>
    {
        /// <summary>
        /// 施工审批单
        /// </summary>
        public virtual Guid ApprovalId { get; set; }
        public virtual Approval Approval { get; set; }
        /// <summary>
        /// 时间选择
        /// </summary>
        public DateTime FillTime { get; set; }
        /// <summary>
        /// 负责人+施工员
        /// </summary>
        public List<DiaryRltBuilderDto> DirectorsRltBuilders { get; set; } = new List<DiaryRltBuilderDto>();
        /// <summary>
        /// 负责人
        /// </summary>
        public List<DiaryRltBuilderDto> Directors { get; set; } = new List<DiaryRltBuilderDto>();
        /// <summary>
        /// 施工员
        /// </summary>
        public List<DiaryRltBuilderDto> Builders { get; set; } = new List<DiaryRltBuilderDto>();
        /// <summary>
        /// 劳务人员
        /// </summary>
        public int MemberNum { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 施工描述
        /// </summary>
        public string Discription { get; set; }
        /// <summary>
        /// 天气{weather_m:null,weather_a:null,weather_e:null}
        /// </summary>
        public string Weathers { get; set; }
        /// <summary>
        /// 温度
        /// </summary>
        public string Temperature { get; set; }
        /// <summary>
        /// 存在的问题
        /// </summary>
        public string Problem { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        public List<DiaryRltFileDto> DiaryRltFiles { get; set; } = new List<DiaryRltFileDto>();
        /// <summary>
        /// 班前讲话视频
        /// </summary>
        public List<DiaryRltFileDto> TalkMedias { get; set; } = new List<DiaryRltFileDto>();
        /// <summary>
        /// 讲话图片
        /// </summary>
        public List<DiaryRltFileDto> Pictures { get; set; } = new List<DiaryRltFileDto>();
        /// <summary>
        /// 施工过程视频
        /// </summary>
        public List<DiaryRltFileDto> ProcessMedias { get; set; } = new List<DiaryRltFileDto>();
        /// <summary>
        /// 物资信息
        /// </summary>
        public List<DiaryRltMaterialDto> DiaryRltMaterials { get; set; } = new List<DiaryRltMaterialDto>();
        /// <summary>
        /// 辅助
        /// </summary>
        public List<DiaryRltMaterialDto> MaterialList { get; set; } = new List<DiaryRltMaterialDto>();
        /// <summary>
        /// 器具
        /// </summary>
        public List<DiaryRltMaterialDto> ApplianceList { get; set; } = new List<DiaryRltMaterialDto>();
        /// <summary>
        /// 机械
        /// </summary>
        public List<DiaryRltMaterialDto> MechanicalList { get; set; } = new List<DiaryRltMaterialDto>();
        /// <summary>
        /// 安防信息
        /// </summary>
        public List<DiaryRltMaterialDto> SecurityProtectionList { get; set; } = new List<DiaryRltMaterialDto>();
  
    }
}
