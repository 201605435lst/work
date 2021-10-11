/**********************************************************************
*******命名空间： SnAbp.Safe.Entities
*******类 名 称： SafeProblemRecordRleFile
*******类 说 明： 安全整改或验证
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

namespace SnAbp.Safe.Entities
{
    /// <summary>
    /// 安全整改或验证
    /// </summary>
    public class SafeProblemRecordRltFile : Entity<Guid>
    {
        public Guid SafeProblemRecordId { get; set; }
        public SafeProblemRecord SafeProblemRecord { get; set; }
        public Guid FileId { get; set; }
        public virtual File.Entities.File File { get; set; }
        //public override object[] GetKeys()
        //{
        //    return new object[] { SafeProblemRecordId, FileId };
        //}
        public SafeProblemRecordRltFile(Guid id)
        {
            Id = id;
        }
    }
}
