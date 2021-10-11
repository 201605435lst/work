using System;
using GenerateLibrary;
using SnAbp.ConstructionBase.Entities;
using SnAbp.Utils.TreeHelper;

namespace SnAbp.Construction.MasterPlans.Entities
{
	/// <summary>
	/// 总体计划和施工计划相同的实体(封装)
	/// </summary>
	public interface IPlanEntity<T> : IGuidKeyTree<T>
	{

		public Guid? SubItemContentId { get; set; }
		public SubItemContent SubItemContent { get; set; }
		
		public string Name { get; set; }
		
		public string Content { get; set; }
		public DateTime PlanStartTime { get; set; }
		public DateTime PlanEndTime { get; set; }
		public double Period { get; set; }
		public bool IsMilestone { get; set; }
	}	
}