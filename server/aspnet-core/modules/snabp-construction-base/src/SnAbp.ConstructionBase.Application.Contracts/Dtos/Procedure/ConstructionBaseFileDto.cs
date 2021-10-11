using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.Procedure
{
	public class ConstructionBaseFileDto : EntityDto<Guid>
	{
		/// <summary>
		/// 文件名称，通过文件所在文件夹的路径拼接：aaa文件/aa.txt
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///  文件大小
		/// </summary>
		public decimal Size { get; set; }

		/// <summary>
		/// 文件地址
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// 文件的路径
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// 文件类型
		/// </summary>

		public string Type { get; set; }
	}
}