using System;
using SnAbp.Construction.Dtos.Plan.PlanContent;
using SnAbp.File.Dtos;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos.Plan.PlanContentRltFile
{

	/// <summary>
	/// 施工计划任务前置表 dto 
	/// </summary>
	public class PlanContentRltFileDto : EntityDto<Guid>
	{

		/// <summary>
		/// 施工计划详情id 
		/// </summary>
		public Guid? PlanContentId {get;set;}
		/// <summary>
		/// 施工计划详情实体
		/// </summary>
		public PlanContentDto PlanContent {get;set;}
		/// <summary>
		/// 文件id 
		/// </summary>
		public Guid? FileId {get;set;}
		/// <summary>
		/// 文件实体 
		/// </summary>
		public FileDto File {get;set;}
	}
}