
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
namespace SnAbp.ConstructionBase.Dtos.Standard
{

	/// <summary>
	/// 工序规范维护 dto 
	/// </summary>
	public class StandardDto : EntityDto<Guid>
	{

		/// <summary>
		/// 规范名称
		/// </summary>
		public string Name {get;set;}
		/// <summary>
		/// 规范编号
		/// </summary>
		public string Code {get;set;}
		/// <summary>
		/// 所属专业
		/// </summary>
		public string Profession {get;set;}
		/// <summary>
		/// 所属专业id
		/// </summary>
		public Guid ProfessionId {get;set;}
	}
}