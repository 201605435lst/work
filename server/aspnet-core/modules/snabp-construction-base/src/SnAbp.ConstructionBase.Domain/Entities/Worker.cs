using SnAbp.MultiProject.MultiProject;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.ConstructionBase.Entities
{
	/// <summary>
	/// 工种信息管理  
	/// </summary>
	public class Worker : FullAuditedEntity<Guid>
	{
		/// <summary>
		/// 这样写是规范 ,继承 Entity<Guid>会自动生成 连续id  
		/// </summary>
		protected Worker()
		{
		}

		public Worker(Guid id)
		{
			Id = id;
		}


		// 名称 
		public string Name { get; set; }
	}
}