using System.ComponentModel;

namespace SnAbp.ConstructionBase.Enums
{
	/// <summary>
	/// 移动类型
	/// </summary>
	public enum MoveType
	{
		[Description("向上移")] Up   = 1,
		[Description("向下移")] Down = 2,
	}
}