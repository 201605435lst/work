using System;
using SnAbp.ConstructionBase.Enums;
using SnAbp.MultiProject.MultiProject;
using Volo.Abp.Domain.Entities.Auditing;
namespace SnAbp.ConstructionBase.Entities
{
	/// <summary>
	/// 工程量清单
	/// </summary>
	public class ConstructionBaseMaterial : FullAuditedEntity<Guid>
	{
		/// <summary>
		/// 这样写是规范 ,继承 Entity<Guid>会自动生成 连续id  
		/// </summary>
		protected ConstructionBaseMaterial()
		{
		}

		/// <summary>
		/// 不加的话报这个错
		/// :“The instance of entity type 'Material' cannot be tracked because another
		/// instance with the same key value for {'Id'} is already being tracked.
		/// When attaching existing entities, ensure that only one entity instance
		/// with a given key value is attached. Consider using
		/// 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to see the conflicting key values.”
		/// ：“无法跟踪实体类型'Material'的实例，因为已经跟踪了另一个具有相同'{'Id'}键值的实例。
		/// 附加现有实体时，请确保仅附加一个具有给定键值的实体实例。
		/// 考虑使用'DbContextOptionsBuilder.EnableSensitiveDataLogging'来查看冲突的键值。”
		/// </summary>
		/// <param name="id"></param>
		public ConstructionBaseMaterial(Guid id)
		{
			Id = id;
		}


		/// <summary>
		/// 工程量编码  
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// 名称及规格  
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 计量单位  个、方、吨、米、m³、袋
		/// </summary>
		public MaterialEnum Unit { get; set; }

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

        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
	}
}