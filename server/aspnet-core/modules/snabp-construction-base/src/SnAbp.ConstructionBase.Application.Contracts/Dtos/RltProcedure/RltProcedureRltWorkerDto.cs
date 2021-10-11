using System;
using SnAbp.ConstructionBase.Dtos.Worker;
using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.RltProcedure
{
	public class RltProcedureRltWorkerDto : EntityDto<Guid>
	{

		/// <summary>
		/// 关联工序id 
		/// </summary>
		public Guid RltProcedureId { get; set; }

		/// <summary>
		/// 工种信息 id  
		/// </summary>
		public Guid WorkerId { get; set; }

		/// <summary>
		/// 工种信息 实体  
		/// </summary>
		public ConstructionBaseWorkerDto Worker { get; set; }
	}
}