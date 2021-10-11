using System;
using System.Collections.Generic;
using SnAbp.ConstructionBase.Dtos.Procedure;
using SnAbp.ConstructionBase.Enums;
using SnAbp.Utils.TreeHelper;
using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.SubItem
{
	/// <summary>
	/// 分布分项-详情 dto
	/// </summary>
	public class SubItemContentDto : FullAuditedEntityDto<Guid>, IGuidKeyTree<SubItemContentDto>
	{
		/// <summary>
		/// 工程名称   
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// 备注    
		/// </summary>
		public string Remarks { get; set; }

		/// <summary>
		/// 节点类型    
		/// </summary>
		public SubItemNodeType NodeType { get; set; }

		/// <summary>
		/// 节点类型    
		/// </summary>
		public string NodeTypeStr { get; set; }


		/// <summary>
		///  分布分项 实体 id 
		/// </summary>
		public Guid? SubItemId { get; set; }

		/// <summary>
		/// 关联 分布分项 实体
		/// </summary>
		public SubItemDto SubItem { get; set; }

		/// <summary>
		/// 排序 (用于上移、下移)
		/// </summary>
		public int Order { get; set; }

		public Guid? ParentId { get; set; }
		public SubItemContentDto Parent { get; set; }
		public List<SubItemContentDto> Children { get; set; } = new List<SubItemContentDto>();


		/// <summary>
		/// 关联施工工序表
		/// </summary>
		public List<ProcedureDto> Procedures { get; set; } = new List<ProcedureDto>();
	}
}