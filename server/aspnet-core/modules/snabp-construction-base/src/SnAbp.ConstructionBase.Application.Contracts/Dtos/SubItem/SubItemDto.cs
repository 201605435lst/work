using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.SubItem
{
	/// <summary>
	/// 分部分项dto
	/// </summary>
	public class SubItemDto : FullAuditedEntityDto<Guid>
	{
		/// <summary>
		/// 分部分项名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 创建时间 string 格式 
		/// </summary>
		public string CreateTime { get; set; }

		/// <summary>
		/// 创建人姓名
		/// </summary>
		public string CreatorName { get; set; }
		/// <summary>
		/// 备注    
		/// </summary>
		public string Remarks { get; set; }

		/// <summary>
		/// 项目名称 (关联项目表 Project.Name)
		/// </summary>
		 public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

		/// <summary>
		/// 是否编制 (是否含有详情 Content )
		/// </summary>
		public bool IsDrawUp { get; set; }

		/// <summary>
		/// 是否含有作业面 (方便工序关联) 
		/// </summary>
		public bool HasWorkSur { get; set; }

		/// <summary>
		/// 分布分项 content Dto 
		/// </summary>
		public SubItemContentDto SubItemContent { get; set; }
	}
}