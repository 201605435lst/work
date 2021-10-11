using System;
using System.Collections.Generic;
using GenerateLibrary;
using Snabp.Common.CodeGenerate;
using SnAbp.ConstructionBase.Enums;
using SnAbp.MultiProject.MultiProject;
using SnAbp.Utils.TreeHelper;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.ConstructionBase.Entities
{
	/// <summary>
	/// 施工区段
	/// </summary>
	public class Section : FullAuditedEntity<Guid>, IGuidKeyTree<Section>
	{
		protected Section()
		{
		}

		public Section(Guid id)
		{
			Id = id;
		}


		/// <summary>
		/// 区段名称
		/// </summary>
		[Comment("区段名称")]
		[Search(SearchType.BlurSearch)]
		[Create(CreateType.StringInput)]
		[Display(DisplayType.String)]
		public string Name { get; set; }

		/// <summary>
		/// 区段描述  
		/// </summary>
		[Comment("区段描述")]
		[Search(SearchType.BlurSearch)]
		[Create(CreateType.StringInput)]
		[Display(DisplayType.String)]
		public string Desc { get; set; }


		[Comment("父级id")]
		[Create(CreateType.GuidSelect)]
		[Display(DisplayType.Guid)]
		public Guid? ParentId { get; set; }
		
		[Comment("父级")]
		[Display(DisplayType.EntityParent)]
		public Section Parent { get; set; }
		
		[Comment("子级列表")]
		[Display(DisplayType.EntityChildrenList)]
		public List<Section> Children { get; set; }
	}
}