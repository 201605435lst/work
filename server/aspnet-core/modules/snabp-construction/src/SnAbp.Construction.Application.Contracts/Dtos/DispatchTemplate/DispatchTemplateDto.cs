
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
namespace SnAbp.Construction.Dtos
{

	/// <summary>
	/// 派工单模板 dto 
	/// </summary>
	public class DispatchTemplateDto : EntityDto<Guid>
	{

		/// <summary>
		/// 模板名称
		/// </summary>
		public string Name {get;set;}
		/// <summary>
		/// 模板说明
		/// </summary>
		public string Description {get;set;}
		/// <summary>
		/// 是否默认
		/// </summary>
		public bool IsDefault {get;set;}
		/// <summary>
		/// 模板备注
		/// </summary>
		public string Remark {get;set;}
	}
}