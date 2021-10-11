using System;
using SnAbp.ConstructionBase.Enums;

namespace SnAbp.ConstructionBase.Dtos.SubItem
{
	/// <summary>
	/// 分布分项-详情 Create dto
	/// </summary>
	public class SubItemContentCreateDto
	{
		/// <summary>
		/// 工程名称   
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// subItem id 可为空 ,如果 不写问号 就 给默认值了……（默认值是00000-00... 传过去数据库又会报错 违反外键约束 ）   
		/// </summary>
		public Guid? SubItemId { get; set; }
		/// <summary>
		/// 备注    
		/// </summary>
		public string Remarks { get; set; }

		/// <summary>
		/// 节点类型    
		/// </summary>
		public SubItemNodeType NodeType { get; set; }
		


		public Guid? ParentId { get; set; }
	}
}