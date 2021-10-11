/**********************************************************************
*******命名空间： SnAbp.Material.Entities.Inventory
*******类 名 称： OutRecord
*******类 说 明： 出库记录
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/2/3 14:39:50
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;

using System;
using System.Collections.Generic;

using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Material.Entities
{
    /// <summary>
    /// 出库记录
    /// </summary>
    public class OutRecord : AuditedEntity<Guid>
    {
        public OutRecord(Guid id) => this.Id = id;
        public void SetId(Guid id) => this.Id = id;

        /// <summary>
        /// 出库编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 出库日期
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
        public List<OutRecordRltFile> OutRecordRltFiles { get; set; }
        /// <summary>
        /// 出库材料
        /// </summary>
        public List<OutRecordRltMaterial> OutRecordRltMaterials { get; set; }
        /// <summary>
        /// 关联二维码
        /// </summary>
        public List<OutRecordRltQRCode> OutRecordRltQRCodes { get; set; }
    }
}
