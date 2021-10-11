using System;
using SnAbp.ConstructionBase.Enums;
using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.Material
{
	/// <summary>
    /// 工程量清单dto
    /// </summary>
	public class ConstructionBaseMaterialDto : EntityDto<Guid>
	{
		/// <summary>
		/// 工程量编码  
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// 名称  
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 计量单位  个、方、吨、米、m³、袋
		/// </summary>
		public MaterialEnum Unit { get; set; }
		/// <summary>
		/// 计量单位 转换为汉字
		/// </summary>
		public string UnitStr { get; set; }

		/// <summary>
		/// 工程材料 是否自己提供  
		/// </summary>
		public bool IsSelf { get; set; }

		/// <summary>
		/// 工程材料 是否甲方提供 
		/// </summary>
		public bool IsPartyAProvide { get; set; }

		/// <summary>
		/// 提前到场天数
		/// </summary>
		public int PresentDays { get; set; }

		/// <summary>
		/// 采购前置天数
		/// </summary>
		public int PrePurchaseDays { get; set; }
		/// <summary>
		/// 数量  ,  关联工序 - material 的时候 要用
		/// </summary>
		public int Count { get; set; }
	}
}