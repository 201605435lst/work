namespace SnAbp.ConstructionBase.Dtos.Worker
{
	/// <summary>
	/// 添加 工种信息 dto 这个 类名字这么长的原因 是因为 和 Sn_CrPlan_Worker 另一个模块 的 workerDto 重名了 
	/// </summary>
	public class ConstructionBaseWorkerCreateDto
	{
		//  工种名称 
		public string Name { get; set; }
	}
}