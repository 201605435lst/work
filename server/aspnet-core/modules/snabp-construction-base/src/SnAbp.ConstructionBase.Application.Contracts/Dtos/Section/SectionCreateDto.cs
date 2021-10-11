
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.Section
{
	/// <summary>
	/// 未写注释 CreateDto (添加Dto) 
	/// </summary>
	public class SectionCreateDto 
	{

		/// <summary>
		/// 区段名称
		/// </summary>
		public String Name {get;set;} 

		/// <summary>
		/// 区段描述
		/// </summary>
		public String Desc {get;set;} 

		/// <summary>
		/// 父级id
		/// </summary>
		public Guid? ParentId {get;set;} 

	}
}
