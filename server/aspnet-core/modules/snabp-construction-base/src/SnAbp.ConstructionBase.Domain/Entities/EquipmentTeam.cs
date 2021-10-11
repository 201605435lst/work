using System;
using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.ConstructionBase.Entities
{
	/// <summary>
	/// 设备台班
	/// </summary>
	public class EquipmentTeam : FullAuditedEntity<Guid>
	{
		/// <summary>
		/// 这样写是规范 ,继承 Entity<Guid>会自动生成 连续id  
		/// </summary>
		protected EquipmentTeam()
		{
		}

		/// <summary>
		/// 不加的话报这个错
		/// :“The instance of entity type 'EquipmentTeam' cannot be tracked because another
		/// instance with the same key value for {'Id'} is already being tracked.
		/// When attaching existing entities, ensure that only one entity instance
		/// with a given key value is attached. Consider using
		/// 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to see the conflicting key values.”
		/// ：“无法跟踪实体类型'EquipmentTeam'的实例，因为已经跟踪了另一个具有相同'{'Id'}键值的实例。
		/// 附加现有实体时，请确保仅附加一个具有给定键值的实体实例。
		/// 考虑使用'DbContextOptionsBuilder.EnableSensitiveDataLogging'来查看冲突的键值。”
		/// </summary>
		/// <param name="id"></param>
		public EquipmentTeam(Guid id)
		{
			Id = id;
		}


		/// <summary>
		/// 设备名称 
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 设备规格 
		/// </summary>
		public string Spec { get; set; }

		/// <summary>
		/// 设备成本
		/// </summary>
		public double Cost { get; set; }

		/// <summary>
		/// 设备类型 关联字典表
		/// </summary>
		public DataDictionary Type { get; set; }

		/// <summary>
		/// 设备类型 id 
		/// </summary>
		public Guid TypeId { get; set; }
	}
}