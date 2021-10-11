
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
namespace SnAbp.ConstructionBase.Dtos.Section
{

	/// <summary>
	/// 未写注释 dto 
	/// </summary>
	public class SectionDto : EntityDto<Guid>
	{

		/// <summary>
		/// 区段名称
		/// </summary>
		public string Name {get;set;}
		/// <summary>
		/// 区段描述
		/// </summary>
		public string Desc {get;set;}
		/// <summary>
		/// 父级id
		/// </summary>
		public Guid? ParentId {get;set;}
		/// <summary>
		/// 父级
		/// </summary>
		public SectionDto Parent {get;set;}
		/// <summary>
		/// 子级列表
		/// </summary>
		public List<SectionDto> Children {get;set;}
	}
}