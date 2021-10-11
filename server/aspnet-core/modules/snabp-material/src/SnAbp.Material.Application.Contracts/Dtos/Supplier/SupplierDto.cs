using SnAbp.Material.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class SupplierDto : EntityDto<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public SupplierType Type { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public SupplierLevel Level { get; set; }

        /// <summary>
        /// 性质
        /// </summary>
        public SupplierProperty Property { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public string Principal { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 法人代表
        /// </summary>
        public string LegalPerson { get; set; }

        /// <summary>
        /// 纳税人识别号
        ///全称 Taxpayer Identification Number
        /// </summary>
        public string TIN { get; set; }

        /// <summary>
        /// 经营范围
        /// </summary>
        public string BusinessScope { get; set; }

        /// <summary>
        /// 开户行名称
        /// </summary>
        public string OpeningBank { get; set; }

        /// <summary>
        /// 开户行账户
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 开户单位
        /// </summary>
        public string AccountOpeningUnit { get; set; }

        /// <summary>
        /// 注册资本
        /// </summary>
        public string RegisteredAssets { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<SupplierRltAccessoryDto> SupplierRltAccessories { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public List<SupplierRltContactsDto> SupplierRltContacts { get; set; }
    }
}
