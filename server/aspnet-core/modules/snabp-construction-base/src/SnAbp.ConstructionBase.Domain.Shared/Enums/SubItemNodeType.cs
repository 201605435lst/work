using System.ComponentModel;

namespace SnAbp.ConstructionBase.Enums
{
	/// <summary>
	/// 分布节点 - 节点类型 
	/// </summary>
	public enum SubItemNodeType
	{
		/// <summary>
		/// 这个 不显示,用来表示 最上层的总工程 
		/// </summary>
		[Description("总工程")] All = 1,
		[Description("单位工程")] Pro,
		[Description("子单位工程")] SubPro,
		[Description("分步工程")] Pos,
		[Description("子分布工程")] SubPos,
		[Description("分项工程")] Item,
		[Description("子分项工程")] SubItem,
		[Description("作业面")] WorkSur,
		[Description("工序")] Procedure,
	}
}