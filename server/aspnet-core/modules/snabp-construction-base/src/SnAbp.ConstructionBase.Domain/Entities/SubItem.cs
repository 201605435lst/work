using System;
using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.ConstructionBase.Entities
{
	/// <summary>
	/// 分部分项
	/// </summary>
	public class SubItem : FullAuditedEntity<Guid>
	{
		protected SubItem() { }
		public SubItem(Guid id) => Id = id;

		/// <summary>
		/// 分部分项 详情 
		/// </summary>
		public SubItemContent SubItemContent { get; set; }

		/// <summary>
		/// 工程名称   
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 备注    
		/// </summary>
		public string Remarks { get; set; }

		public IdentityUser Creator { get; set; }
	}
}