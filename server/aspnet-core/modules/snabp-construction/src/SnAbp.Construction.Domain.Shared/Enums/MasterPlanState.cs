using System.ComponentModel;

namespace SnAbp.Construction.Enums
{
	/// <summary>
	/// 总体计划审批状态
	/// </summary>
	public enum MasterPlanState
	{
		All=0, // 所有
		[Description("待提交")]
		ToSubmit = 1,
		[Description("审核中")]
		OnReview = 2,
		[Description("审核通过")]
		Pass = 3,
		[Description("审核未通过")]
		UnPass = 4,
	}
}