using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Safe.Dtos
{
   public class SafeSpeechVideoSimpleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 施工部位
        /// </summary>
        public string Site { get; set; }
        /// <summary>
        /// 施工内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 施工日期
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 讲话视频
        /// </summary>
        public virtual File.Entities.File Video { get; set; }
        public virtual Guid? VideoId { get; set; }
    }
}
