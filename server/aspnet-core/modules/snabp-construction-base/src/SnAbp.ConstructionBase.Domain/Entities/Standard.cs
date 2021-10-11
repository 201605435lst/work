using System;
using GenerateLibrary;
using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.ConstructionBase.Entities
{
	/// <summary>
	/// 工序规范维护  
	/// </summary>
	[Comment("工序规范维护")]
	public class Standard : FullAuditedEntity<Guid>
	{
		/// <summary>
		/// 这样写是规范 
		/// </summary>
		protected Standard() { }
		public Standard(Guid id) { Id = id; }
		/// <summary>
		/// 规范名称
		/// </summary>
		[Comment("规范名称")]
		[Search(SearchType.BlurSearch)]
		[Create(CreateType.StringInput)]
		[Display(DisplayType.String)]
		public string Name { get; set; }

		/// <summary>
		/// 规范编号
		/// </summary>
		[Comment("规范编号")]
		[Search(SearchType.BlurSearch)]
		[Create(CreateType.StringInput)]
		[Display(DisplayType.String)]
		public string Code { get; set; }

		/// <summary>
		/// 所属专业 
		/// </summary>
		[Comment("所属专业")]
		[Display(DisplayType.Dictionary)]
		public DataDictionary Profession { get; set; }

		/// <summary>
		/// 所属专业 id  
		/// </summary>
		[Comment("所属专业id")]
		[Search(SearchType.IdSearch)]
		[Create(CreateType.GuidSelect)]
		[Display(DisplayType.Guid)]
		public Guid ProfessionId { get; set; }
	}
}