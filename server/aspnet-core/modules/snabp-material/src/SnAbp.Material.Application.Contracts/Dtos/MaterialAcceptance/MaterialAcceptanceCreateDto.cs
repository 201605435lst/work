using SnAbp.Material.Entities;
using SnAbp.Material.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
   public class MaterialAcceptanceCreateDto
    {
        /// <summary>
        /// 验收单与采购清单关联表
        /// </summary>
        public List<MaterialAcceptanceRltPurchaseDto> MaterialAcceptanceRltPurchases { get; set; }
        /// 检测机构（数据字典）
        /// </summary>
        public virtual Guid TestingOrganizationId { get; set; }
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
        /// 登记人
        /// </summary>
        public virtual Guid CreatorId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 验收单与物资关联表
        /// </summary>
        public List<MaterialAcceptanceRltMaterialCreateDto> MaterialAcceptanceRltMaterials { get; set; }
        /// <summary>
        /// 验收单与二维码关联表
        /// </summary>
        public List<MaterialAcceptanceRltQRCodeCreateDto> MaterialAcceptanceRltQRCodes { get; set; }
        /// <summary>
        /// 关联资料
        /// </summary>
        public List<MaterialAcceptanceRltFileCreateDto> MaterialAcceptanceRltFiles { get; set; }
    }
}
