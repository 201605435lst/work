using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using SnAbp.ConstructionBase.Enums;
using SnAbp.MultiProject.MultiProject;
using SnAbp.Utils.TreeHelper;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.ConstructionBase.Entities
{
	/// <summary>
	/// 分部分项 - 详情 
	/// </summary>
	public class SubItemContent : FullAuditedEntity<Guid>, IGuidKeyTree<SubItemContent>
	{
		protected SubItemContent()
		{
		}

		public SubItemContent(Guid id)
		{
			Id = id;
		}

		/// <summary>
		///  分部分项 实体 id 
		/// </summary>
		public Guid? SubItemId { get; set; }

		/// <summary>
		/// 关联 分部分项 实体
		/// </summary>
		public SubItem SubItem { get; set; }

		/// <summary>
		/// 备注    
		/// </summary>
		public string Remarks { get; set; }

		/// <summary>
		/// 节点名称   
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 节点类型    
		/// </summary>
		public SubItemNodeType NodeType { get; set; }

		/// <summary>
		/// 排序 (用于上移、下移)
		/// </summary>
		public int Order { get; set; }


		public Guid? ParentId { get; set; }
		public SubItemContent Parent { get; set; }
		public List<SubItemContent> Children { get; set; }
		/// <summary>
		/// 关联施工工序表
		/// </summary>
		public List<SubItemContentRltProcedure> SubItemContentRltProcedures { get; set; }
	}
}
