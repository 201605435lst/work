using System.ComponentModel;

namespace SnAbp.Construction.Enums
{
	/// <summary>
	/// 甘特图 item 编辑 标记
	/// </summary>
	public enum GanttItemState
	{
		[Description("未修改")]
		UnModify= 1, //未修改
		[Description("已编辑")]
		Edit= 2, //已编辑
		[Description("已添加")]
		Add= 3, //已添加
		[Description("已删除")]
		Delete= 4, //已删除
		
	}
}