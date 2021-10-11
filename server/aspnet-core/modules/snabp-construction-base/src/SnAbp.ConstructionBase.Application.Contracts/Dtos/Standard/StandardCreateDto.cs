
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.Standard
{
	/// <summary>
	/// 工序规范维护 CreateDto (添加Dto) 
	/// </summary>
	public class StandardCreateDto 
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
