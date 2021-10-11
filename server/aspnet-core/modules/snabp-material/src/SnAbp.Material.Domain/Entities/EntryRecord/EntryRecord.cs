/**********************************************************************
*******命名空间： SnAbp.Material.Entities.Inventory
*******类 名 称： EntryRecord
*******类 说 明： 入库记录表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/2/3 14:37:03
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;

using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Material.Entities
{
    /// <summary>
    /// 入库记录表
    /// </summary>
    public class EntryRecord : AuditedEntity<Guid>
    {
        public EntryRecord(Guid id) => this.Id = id;
        public void SetId(Guid id) => this.Id = id;

        /// <summary>
        /// 入库编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 关联仓库
        /// </summary>
        public virtual Guid PartitionId { get; set; }
        public virtual Partition Partition { get; set; }

        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        /// <summary>
        /// 登记人
        /// </summary>
        public virtual IdentityUser Creator { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 关联资料
        /// </summary>
        public List<EntryRecordRltFile> EntryRecordRltFiles { get; set; }
        /// <summary>
        /// 入库材料
        /// </summary>
        public List<EntryRecordRltMaterial> EntryRecordRltMaterials { get; set; }
        /// <summary>
        /// 关联二维码
        /// </summary>
        public List<EntryRecordRltQRCode> EntryRecordRltQRCodes { get; set; }

    }
}
