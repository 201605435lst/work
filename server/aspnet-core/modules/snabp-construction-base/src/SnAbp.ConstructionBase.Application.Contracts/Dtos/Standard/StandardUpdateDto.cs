
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.Standard
{
	/// <summary>
	/// 工序规范维护 UpdateDto (更新Dto) 
	/// </summary>
	public class StandardUpdateDto 
	{

		/// <summary>
		/// 规范名称
		/// </summary>
		public String Name {get;set;} 

		/// <summary>
		/// 规范编号
		/// </summary>
		public String Code {get;set;} 

		/// <summary>
		/// 所属专业id
		/// </summary>
		public Guid ProfessionId {get;set;} 

	}
}
