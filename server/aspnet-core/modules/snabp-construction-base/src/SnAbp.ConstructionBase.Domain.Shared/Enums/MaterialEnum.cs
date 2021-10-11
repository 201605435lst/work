using System.ComponentModel;

namespace SnAbp.ConstructionBase.Enums
{
	/// <summary>
	/// 工程量 材料计量单位 
	/// </summary>
	public enum MaterialEnum
	{
		[Description("个")] A = 1,
		[Description("方")] Square,
		[Description("吨")] Ton,
		[Description("米")] Meter,
		[Description("m³")] CubicMeter,
		[Description("袋")] Bag,
	}
}