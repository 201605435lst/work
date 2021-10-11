using SnAbp.Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Schedule.Dtos
{
    public class SpeachVideoDto:EntityDto<Guid>
    {
        /// <summary>
        /// 施工部位
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 施工内容
        /// </summary>
        public string Schedule { get; set; }
        /// <summary>
        /// 施工日期    
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 视频
        /// </summary>
        public List<DiaryRltFile> TalkMedias { get; set; } = new List<DiaryRltFile>();
    }
}
