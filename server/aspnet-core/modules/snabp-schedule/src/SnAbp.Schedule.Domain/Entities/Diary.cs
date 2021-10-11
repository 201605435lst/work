using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Schedule.Entities
{
    public class Diary : FullAuditedEntity<Guid>
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
        /// 编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 负责人+施工员
        /// </summary>
        public List<DiaryRltBuilder> DirectorsRltBuilders { get; set; }
        /// <summary>
        /// 劳务人员
        /// </summary>
        public int MemberNum { get; set; }
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
        /// 班前讲话视频+图片+施工视频
        /// </summary>
        public List<DiaryRltFile> DiaryRltFiles { get; set; }
        /// <summary>
        /// 物资信息
        /// </summary>
        public List<DiaryRltMaterial> DiaryRltMaterials { get; set; }

        public Diary(Guid id)
        {
            Id = id;
        }
        public void SetId(Guid id)
        {
            Id = id;
        }
        public Diary() { }
    }
}
