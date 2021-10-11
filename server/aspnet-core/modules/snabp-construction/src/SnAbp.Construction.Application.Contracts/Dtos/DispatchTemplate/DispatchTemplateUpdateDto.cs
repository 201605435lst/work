
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos
{
	/// <summary>
	/// 派工单模板 UpdateDto (更新Dto) 
	/// </summary>
	public class DispatchTemplateUpdateDto 
	{

		/// <summary>
		/// 模板名称
		/// </summary>
		public String Name {get;set;} 
		/// <summary>
		/// 模板说明
		/// </summary>
		public String Description {get;set;} 
		/// <summary>
		/// 是否默认
		/// </summary>
		public bool IsDefault {get;set;}
		/// <summary>
		/// 模板备注
		/// </summary>
		public String Remark {get;set;} 
	}
}
