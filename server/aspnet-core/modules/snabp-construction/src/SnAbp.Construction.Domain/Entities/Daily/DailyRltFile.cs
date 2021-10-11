/**********************************************************************
*******命名空间： SnAbp.Construction.Entities.Daily
*******类 名 称： DailyRltFile
*******类 说 明： 施工日志文件
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 7/16/2021 9:32:54 AM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Construction.Entities
{
    /// <summary>
    ///  施工日志文件
    /// </summary>
    public class DailyRltFile : Entity<Guid>
    {
        public virtual Guid DailyId { get; set; }
        public virtual Daily Daily { get; set; }
        public virtual Guid FileId { get; set; }
        public virtual File.Entities.File File { get; set; }

        public DailyRltFile(Guid id)
        {
            Id = id;
        }
    }
}
