using System;
using GenerateLibrary;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Construction.Plans
{
	/// <summary>
	/// 施工计划任务前置表
	/// </summary>
	[Comment("施工计划任务前置表")]
	public class PlanContentRltFile: FullAuditedEntity<Guid>
	{
		protected PlanContentRltFile() { }
		public PlanContentRltFile(Guid id) => Id = id;
		
		/// <summary>
		/// 施工计划详情id 
		/// </summary>
		[Comment("施工计划详情id ")]
		[Display(DisplayType.Guid)]
		public Guid? PlanContentId { get; set; }

		/// <summary>
		/// 施工计划详情实体 
		/// </summary>
		[Comment("施工计划详情实体")]
		[Display(DisplayType.Entity)]
		public PlanContent PlanContent { get; set; }
		
		/// <summary>
		/// 文件id 
		/// </summary>
		[Comment("文件id ")]
		[Display(DisplayType.Guid)]
		public Guid? FileId { get; set; }
		/// <summary>
		/// 文件实体 
		/// </summary>
		[Comment("文件实体 ")]
		[Display(DisplayType.Entity)]
		public File.Entities.File File { get; set; }

	}
}