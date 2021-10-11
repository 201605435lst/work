using System;
using SnAbp.ConstructionBase.Dtos.Procedure;
using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.RltProcedure
{
	public class RltProcedureRltFileDto : EntityDto<Guid>
	{
		/// <summary>
		/// 关联工序id 
		/// </summary>
		public Guid RltProcedureId { get; set; }
		/// <summary>
		/// 工程量清单 id  
		/// </summary>
		public Guid FileId { get; set; }
		/// <summary>
		/// 工程量清单 实体  
		/// </summary>
		public virtual ConstructionBaseFileDto File { get; set; }

	}
}