using System;

namespace GenerateLibrary
{
	/// <summary>
	/// 关联查询 的特性 
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class IncludeAttribute : Attribute
	{
		public IncludeAttribute()
		{
		}

		/// <summary>
		/// 完整关联,嵌套用的(User.Student.Teacher)
		/// </summary>
		public string FullInclude { get; }

		public IncludeAttribute(string fullInclude)
		{
			FullInclude = fullInclude;
		}
	}
}
