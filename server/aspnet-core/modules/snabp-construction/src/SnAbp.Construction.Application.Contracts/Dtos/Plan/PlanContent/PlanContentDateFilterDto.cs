namespace SnAbp.Construction.Dtos.Plan.PlanContent
{
	/// <summary>
	/// 施工计划详情 树 日期筛选dto 
	/// </summary>
	public class PlanContentDateFilterDto 
	{
		/// <summary>
		/// 年份 
		/// </summary>
		public int? Year { get; set; }
		/// <summary>
		/// 季度 
		/// </summary>
		public int? Quarter { get; set; }
		/// <summary>
		/// 月份 
		/// </summary>
		public int? Month { get; set; }
		/// <summary>
		/// 周数 开始 天 
		/// </summary>
		public int? DayStart { get; set; }
		/// <summary>
		/// 周数 结束 天 
		/// </summary>
		public int? DayEnd { get; set; }
	}
}