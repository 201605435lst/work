/**********************************************************************
*******命名空间： SnAbp.Safe.Entities
*******类 名 称： SafeProblemRltFile
*******类 说 明： 安全问题文件关联
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/4/29 19:00:30
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities;

namespace SnAbp.Safe.Entities
{
    /// <summary>
    /// $$
    /// </summary>
    public class SafeProblemRltFile : Entity<Guid>
    {
        public virtual Guid SafeProblemId { get; set; }
        public virtual SafeProblem SafeProblem { get; set; }
        public virtual Guid FileId { get; set; }
        public virtual File.Entities.File File { get; set; }
        public SafeProblemRltFile(Guid id)
        {
            Id = id;
        }
    }
}
