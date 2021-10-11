using System;
using SnAbp.ConstructionBase.Dtos.SubItem;
using SnAbp.Utils.TreeHelper;

namespace SnAbp.Construction.Dtos.MasterPlan.MasterPlanContent
{
	/// <summary>
	/// 总体计划和施工计划相同的Dto(封装)
	/// </summary>
	public interface IPlanDto<TDto> : IGuidKeyTree<TDto>,ICloneable
	{

		public Guid? SubItemContentId { get; set; }
		public SubItemContentDto SubItemContent { get; set; }
		
		public string Name { get; set; }
		
		public string Content { get; set; }
		public DateTime PlanStartTime { get; set; }
		public DateTime PlanEndTime { get; set; }
		public double Period { get; set; }
		public bool IsMilestone { get; set; }
	}
}