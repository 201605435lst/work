using SnAbp.Identity;
using SnAbp.Material.Enums;
using SnAbp.MultiProject.MultiProject;

using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

/*'materialPlayName', //采购单
  'testingOrganizationId', //检测机构
  'code', //报告编号
  'testingType', //检测类型
  'date', //报告日期
  'remark', //备注*/

namespace SnAbp.Material.Entities
{
    /// <summary>
    /// 物资验收实体
    /// </summary>
    public class MaterialAcceptance : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 验收单与采购清单关联表
        /// </summary>
        public List<MaterialAcceptanceRltPurchase> MaterialAcceptanceRltPurchases { get; set; }

        /// 检测机构（数据字典）
        /// </summary>
        public virtual Guid TestingOrganizationId { get; set; }
        public virtual DataDictionary TestingOrganization { get; set; }
        /// <summary>
        /// 报告编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 检测类型
        /// </summary>
        public TestingType TestingType { get; set; }
        /// <summary>
        /// 检测状态
        /// </summary>
        public TestingStatus TestingStatus { get; set; }
        /// <summary>
        /// <summary>
        /// 验收时间
        /// </summary>
        public DateTime? ReceptionTime { get; set; }
        /// <summary>
        /// <summary>
        ///登记人
        /// </summary>
        public IdentityUser Creator { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 验收单与物资关联表
        /// </summary>
        public List<MaterialAcceptanceRltMaterial> MaterialAcceptanceRltMaterials { get; set; }
        /// <summary>
        /// 验收单与二维码关联表
        /// </summary>
        public List<MaterialAcceptanceRltQRCode> MaterialAcceptanceRltQRCodes { get; set; }
        /// <summary>
        /// 关联资料
        /// </summary>
        public List<MaterialAcceptanceRltFile> MaterialAcceptanceRltFiles { get; set; }

        public MaterialAcceptance(Guid id)
        {
            Id = id;
        }
        public MaterialAcceptance() { }
        public void SetId(Guid id)
        {
            Id = id;
        }
        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
