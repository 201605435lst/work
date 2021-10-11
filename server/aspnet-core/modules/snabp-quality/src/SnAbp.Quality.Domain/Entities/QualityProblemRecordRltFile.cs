/**********************************************************************
*******命名空间： SnAbp.Quality.Entities
*******类 名 称： QualityProblemRecordRleFile
*******类 说 明： 质量整改或验证
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/4/29 19:10:24
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities;

namespace SnAbp.Quality.Entities
{
    /// <summary>
    /// $$
    /// </summary>
    public class QualityProblemRecordRltFile : Entity<Guid>
    {
        public Guid QualityProblemRecordId { get; set; }
        public QualityProblemRecord QualityProblemRecord { get; set; }
        public Guid FileId { get; set; }
        public virtual File.Entities.File File { get; set; }
        public QualityProblemRecordRltFile(Guid id)
        {
            Id = id;
        }
    }
}
